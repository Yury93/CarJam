using _Project.Scripts.GameLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public class CarPath : MonoBehaviour
    {
        [SerializeField] private List<Transform> _points;
        [Range(0, 4), SerializeField] private int _lowerLine = 4;
        [Range(0, 4), SerializeField] private int _upLine = 2;
        [SerializeField] private int _upPoint1 = 2;
        [SerializeField] private int _upPoint2 = 3;
        public static CarPath instance;// избавитьс€ от синглтона
        [field: SerializeField] public List<CarStand> Stands { get; private set; }
        private void Start()
        {
            instance = this;
        }
        public List<Vector3> GetPath(CarMover carMover)
        {
            var freeStand = Stands.FirstOrDefault(s => s.Free);
            if (freeStand == null)
            {
                Debug.LogError("Ќет свободных остановок");
                return new List<Vector3>();
            }

            List<Vector3> path = new List<Vector3>();//сделать структурку
            Vector3 carPosition = carMover.transform.position;
            Vector3 rayDirection = carMover.transform.forward;
            Vector3 intersectionPoint = Vector3.zero;
            bool foundIntersection = false;
            int intersectedSegmentIndex = -1;
            bool isLowerLine = false;
            bool isUpLine = false;

            AddFirstPoint(path, carPosition, rayDirection, ref intersectionPoint, ref foundIntersection, out intersectedSegmentIndex);

            if (foundIntersection)
            {
                isLowerLine = (intersectedSegmentIndex == _points.Count - _lowerLine);
                isUpLine = (intersectedSegmentIndex == _points.Count - _upLine);
            }
            if (isLowerLine)
            {
                AddDownPoint(path, intersectionPoint);
            }
            if (isUpLine == false)
            {
                AddUpPoint(path);
            }
            path.Add(freeStand.RoadPointIn.position);
            path.Add(freeStand.transform.position);
            carMover.SetStandTarget(freeStand);
            return path;
        }

        private void AddUpPoint(List<Vector3> path)
        {
            float firstUpPoint = Vector3.Distance(path.Last(), _points[_upPoint1].position);
            float secondUpPoint = Vector3.Distance(path.Last(), _points[_upPoint2].position);
            if (firstUpPoint < secondUpPoint)
            {
                path.Add(_points[_upPoint1].position);
            }
            else
            {
                path.Add(_points[_upPoint2].position);
            }
        }

        private void AddFirstPoint(List<Vector3> path,
            Vector3 carPosition,
            Vector3 rayDirection,
            ref Vector3 intersectionPoint,
            ref bool foundIntersection,
            out int intersectedSegmentIndex)
        {
            intersectedSegmentIndex = 0;
            for (int i = 0; i < _points.Count; i++)
            {
                Vector3 a = _points[i].position;
                Vector3 b = _points[0].position;
                if (i < _points.Count - 1)
                    b = _points[i + 1].position;

                if (RaySegmentIntersection(carPosition, rayDirection, a, b, out intersectionPoint))
                {
                    path.Add(intersectionPoint);
                    foundIntersection = true;
                    intersectedSegmentIndex = i;
                    break;
                }
            }
        }
        private void AddDownPoint(List<Vector3> path, Vector3 intersectionPoint)
        {
            float minimalDistance = Vector3.Distance(_points[0].position, intersectionPoint);
            int minimalDistancePoint = 0;
            for (int i = 0; i < _points.Count; i++)
            {
                var distanceToPoint = Vector3.Distance(_points[i].position, intersectionPoint);
                if (minimalDistance > distanceToPoint)
                {
                    minimalDistance = distanceToPoint;
                    minimalDistancePoint = i;
                }
            }
            Transform minDistancePoint = _points[minimalDistancePoint];
            path.Add(minDistancePoint.position);
        }
        private bool RaySegmentIntersection(Vector3 rayOrigin, Vector3 rayDir, Vector3 pointA, Vector3 pointB, out Vector3 intersection)
        {
            intersection = Vector3.zero;
            Vector3 segmentDir = pointB - pointA;
            Vector3 crossRaySeg = Vector3.Cross(rayDir, segmentDir);

            if (Mathf.Approximately(crossRaySeg.magnitude, 0))
                return false;

            Vector3 diff = pointA - rayOrigin;
            float t = Vector3.Dot(Vector3.Cross(diff, segmentDir), crossRaySeg) / crossRaySeg.sqrMagnitude;
            float u = Vector3.Dot(Vector3.Cross(diff, rayDir), crossRaySeg) / crossRaySeg.sqrMagnitude;

            if (t >= 0 && u >= 0 && u <= 1)
            {
                intersection = rayOrigin + rayDir * t;
                return true;
            }
            return false;
        }
        private void Update()
        {
            ShowDebugLine();
        }
        private void ShowDebugLine()
        {
            for (int i = 0; i < _points.Count; i++)
            {

                Vector3 a = _points[i].position;
                Vector3 b = _points[0].position;
                if (i < _points.Count - 1)
                    b = _points[i + 1].position;
                Debug.DrawLine(a, b, (i == _points.Count - _lowerLine) ? Color.red : Color.green, 2f);
                Debug.DrawLine(a, b, (i == _points.Count - _upLine) ? Color.red : Color.green, 2f);
            }
        }
    }
}
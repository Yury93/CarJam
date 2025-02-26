using _Project.Scripts.GameLogic;
using _Project.Scripts.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyObject : MonoBehaviour
{
    [SerializeField] private List<CarStand> carStands;
    [SerializeField] private Transform rotator;
    [SerializeField] private float carYOffset;

    private Car car;
    private Vector3 startCarPosition;
    private Vector3 myStartPosition;
    private bool isMove;
    public void Init(List<CarStand> stands)
    {
        myStartPosition = transform.position;
        this.carStands = stands;
    }
    private void Update()
    {
      if(isMove)  rotator.Rotate(0, 500 * Time.deltaTime, 0);
    }
    public void ToCar(Car car)
    {
        this.car = car;
        startCarPosition = car.transform.position;
        StartCoroutine(MoveToCar());
    }

    private IEnumerator MoveToCar()
    {
        isMove = true;
        yield return MoveToPosition(new Vector3(car.transform.position.x, transform.position.y, car.transform.position.z));
        yield return MoveToStand();
        isMove = false;
    }

    private IEnumerator MoveToStand()
    {
        CarStand freeStand = null;
       var mover = this.car.GetComponent<CarMover>();
        var car =  new List<Transform>() { this.car.transform };
        while (freeStand == null || Vector3.Distance(transform.position, new Vector3(freeStand.transform.position.x, transform.position.y, freeStand.transform.position.z)) > Constants.EPSILON)
        {
            freeStand = carStands.Find(c => c.Free);
            if (freeStand == null)
            {
                yield return new WaitUntil(() => carStands.Exists(c => c.Free));
                freeStand = carStands.Find(c => c.Free);
            }
            if(freeStand.Free)  mover.SetStandTarget(freeStand);
            yield return MoveToPosition(new Vector3(freeStand.transform.position.x, transform.position.y, freeStand.transform.position.z), car);
        }
        var eulerCar = this.car.transform.localRotation.eulerAngles;
        this.car.transform.localRotation = Quaternion.Euler(new Vector3(0,eulerCar.y,eulerCar.z));
        this.car.transform.localPosition = new Vector3(this.car.transform.localPosition.x, 0, this.car.transform.localPosition.z);
       yield return MoveToPosition(myStartPosition);
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, List<Transform> transformChildrens = null)
    {
        while (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z)) > Constants.EPSILON)
        {
            RotateTowards(targetPosition,transform);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), 10f * Time.deltaTime);
            if (transformChildrens != null)
            {
                transformChildrens.ForEach(c => RotateTowards(targetPosition, c.transform));
                transformChildrens.ForEach(c => c.transform.position = new Vector3(transform.position.x, carYOffset, transform.position.z));
            }
            yield return null;
        }
    }

    private void RotateTowards(Vector3 targetPosition, Transform transformRotated)
    {
        Vector3 direction = targetPosition - transformRotated.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transformRotated.rotation = Quaternion.Slerp(transformRotated.rotation, targetRotation, 3 * Time.deltaTime);
    }
}
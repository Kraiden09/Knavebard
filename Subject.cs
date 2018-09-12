using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour {
    bool subReady = false, subInit = false;
    protected List<IObserver> observerList;

    public void Subscribe(IObserver o) {
        SubInit();
        observerList.Add(o);
        if (subReady) NotifyAll();
    }

    public void NotifyAll() {
        subReady = true;
        SubInit();
        if (observerList.Count > 0) {
            IObserver[] observerArr = observerList.ToArray();
            for (int i = 0; i < observerArr.Length; i++) {
                observerArr[i].UpdateObserver(this);
            }
            observerList.Clear();
        }
    }

    void SubInit() {
        if (!subInit) {
            observerList = new List<IObserver>();
            subInit = true;
        }
    }
}

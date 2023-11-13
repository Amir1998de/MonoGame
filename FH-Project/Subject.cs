using FH_Projekt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

public abstract class Subject
{
    private List<IObserver> observers = new List<IObserver>();



    protected void GetNotified(PlayerActions data)
    {
        observers.ForEach(a => a.OnNotify(data));
    }

    public void AddObserver(IObserver io)
    {
        observers.Add(io);
    }

    public void RemoveObserver(IObserver io)
    {
        observers.Remove(io);
    }
}


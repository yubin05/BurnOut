using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// View -> 화면에 보여줄 UI 요소들, Presenter를 통해 데이터를 전달받음
public interface IView<M>
{
    public void UpdateUI(M model);
}
public abstract class View<P, M> : MonoBehaviour, IView<M> where P : Presenter<M>, new() where M : Model, new()
{
    protected P presenter;

    public virtual void Init()
    {
        presenter = new P();
        presenter.Init(this);
    }

    public abstract void UpdateUI(M model);
}

// Presenter -> Model에 데이터를 담아 View로 전달해주는 매개체
public class Presenter<M> where M : Model, new()
{
    protected IView<M> view;
    protected M model;

    public virtual void Init(IView<M> view)
    {
        this.view = view;
        model = new M();

        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        view.UpdateUI(model);
    }
}

// Model -> View에 뿌려줄 데이터를 갖고있는 클래스 (Presenter를 통해 전달)
public class Model
{
}

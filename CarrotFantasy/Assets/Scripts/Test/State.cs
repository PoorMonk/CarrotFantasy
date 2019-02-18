using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Context cText = new Context("Harkey");
		cText.SetState(new Eat(cText));
		cText.Handle();
		cText.SetState(new Sleep(cText));
		cText.Handle();
	}
	
}

public class Context
{
	private IState m_state;
	private string m_name;

    public string Name { get { return m_name;} }

    public Context(string name)
	{
		m_name = name;
	}

    public void SetState(IState state)
	{
		m_state = state;
	}

	public void Handle()
	{
		m_state.Handle();
	}
}

public interface IState {
	void Handle();
}

public class Eat : IState
{
	public Context m_context;

	public Eat(Context cText)
	{
		m_context = cText;
	}
    public void Handle()
    {
        Debug.Log(m_context.Name + "在吃饭");
    }
}

public class Sleep : IState
{
	public Context m_context;

	public Sleep(Context cText)
	{
		m_context = cText;
	}
    public void Handle()
    {
        Debug.Log(m_context.Name + "在睡觉");
    }
}

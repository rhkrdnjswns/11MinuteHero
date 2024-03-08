using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritTest : MonoBehaviour
{
    public Computer computer;

    public Tv tv; // ����� ���, �Ҹ� ���

    private void Awake()
    {
        computer = new Computer("��ǻ��");
        tv = new Tv("Ƽ��");

        tv.PrintName();
        tv.moniter.PrintName();
        tv.moniter.OutDisplay();
    }

    private void Update()
    {
       // if
    }
}

public class Machine
{
    protected string name;

    public void PrintName()
    {
        Debug.Log(name);
    }

    public Machine(string name)
    {
        this.name = name;
    }
}

public class Computer : Machine //���� �ѱ�, ���콺 �Է�, Ű���� �Է�, �Ҹ� ���, ȭ�� ���
{
    public Moniter moniter;
    public Computer(string name) : base(name)
    {

    }
}
public class Tv : Machine
{
    public Moniter moniter;
    public Speaker speaker;
    public int channel; //ä�� ����
    public Tv(string name) : base (name)
    {
        channel = 0;
        moniter = new Moniter("�����");
        speaker = new Speaker("����Ŀ");
    }
}



public class Moniter : Machine
{
    public Moniter(string name) : base(name)
    {

    }
    public void OutDisplay()
    {
        Debug.Log("ȭ�� ���");
    }
}
public class Speaker : Machine
{
    public Speaker(string name) : base(name)
    {

    }
    public void OutSound()
    {
        Debug.Log("�Ҹ� ���");
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritTest : MonoBehaviour
{
    public Computer computer;

    public Tv tv; // 모니터 출력, 소리 출력

    private void Awake()
    {
        computer = new Computer("컴퓨터");
        tv = new Tv("티비");

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

public class Computer : Machine //전원 켜기, 마우스 입력, 키보드 입력, 소리 출력, 화면 출력
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
    public int channel; //채널 정보
    public Tv(string name) : base (name)
    {
        channel = 0;
        moniter = new Moniter("모니터");
        speaker = new Speaker("스피커");
    }
}



public class Moniter : Machine
{
    public Moniter(string name) : base(name)
    {

    }
    public void OutDisplay()
    {
        Debug.Log("화면 출력");
    }
}
public class Speaker : Machine
{
    public Speaker(string name) : base(name)
    {

    }
    public void OutSound()
    {
        Debug.Log("소리 출력");
    }
}



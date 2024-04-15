using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomPriorityQueue<T> //커스터마이징 우선순위 큐
                                    //Dequeue와 Peek이 필요 없다.
                                    //해당 자료구조는 캐싱된 몬스터 리스트를 플레이어와의 거리에 따라 정렬할 때 초기화 후 사용한다.
{
    [SerializeField] private List<T> heap = new List<T>(); //힙 구조의 리스트
    private IComparer<T> comparer; //컴페어 인터페이스 참조 (컴페어 인터페이스를 상속하는 클래스의 참조임)

    public T this[int index]
    {
        get
        {
            if (index >= 0 && index < heap.Count) //인덱스가 유효한 경우에만 반환
            {
                return heap[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index is Out of Range."); //유효하지 않은 경우 예외 처리
            }
        }
    }
    public int Count
    {
        get
        {
            return heap.Count;
        }
    }
    public CustomPriorityQueue()
    {
        comparer = Comparer<T>.Default;
    }
    public CustomPriorityQueue(IComparer<T> comparer) //생성자
    {
        this.comparer = comparer;
    }

    public void Enqueue(T item) //최소 힙 구조로 인큐가 일어남
    {
        heap.Add(item);
        int index = heap.Count - 1; //현재 노드의 인덱스를 가져옴

        while(index > 0)
        {
            int parentIndex = (index - 1) / 2; //부모 노드의 인덱스 가져옴
            if (comparer.Compare(heap[index], heap[parentIndex]) >= 0) //현재 노드와 부모 노드의 우선순위 조건 검사 (노드 스왑이 필요 없는 경우에는 반복문 탈출)
            {
                break;
            }

            T temp = heap[index]; //노드 스왑이 필요한 경우의 처리
            heap[index] = heap[parentIndex];
            heap[parentIndex] = temp;

            index = parentIndex;
        }
    }
    public T Dequeue() //디큐. 최소값(0번 노드)를 빼준 후, 빠진 자리를 다시 최소값으로 채우는 연산 실행. 후 빼놨던 최소값 노드 반환
    {
        if(heap.Count == 0)
        {
            throw new InvalidOperationException("Priority Queue is Empty");
        }

        T root = heap[0]; //루트 노드(최소값)을 빼줌
        heap[0] = heap[heap.Count - 1]; //루트 노드 위치에 맨 끝 노드를 복사하여 넣어줌
        heap.RemoveAt(heap.Count - 1); //맨 끝 노드 원본 삭제

        int index = 0; //루트 인덱스
        while (true)
        {
            int leftChildIndex = index * 2 + 1; //현재 노드의 왼쪽 자식 노드 인덱스
            int rightChildIndex = index * 2 + 2; //현재 노드의 오른쪽 자식 노드 인덱스
            int smallestChildIndex = index; //가장 값이 작은 노드의 인덱스 (현재 인덱스로 초기화)

            if (leftChildIndex < heap.Count && comparer.Compare(heap[leftChildIndex], heap[smallestChildIndex]) < 0) //왼쪽 자식 노드가 현재 노드보다 값이 작으면 
                smallestChildIndex = leftChildIndex; //최소값 노드를 가리키는 인덱스를 왼쪽 자식 노드 인덱스로 해줌

            if (rightChildIndex < heap.Count && comparer.Compare(heap[rightChildIndex], heap[smallestChildIndex]) < 0)//오른쪽 자식 노드가 현재 노드보다 값이 작으면
                smallestChildIndex = rightChildIndex; //최소값 노드를 가리키는 인덱스를 오른쪽 자식 노드 인덱스로 해줌

            if (smallestChildIndex == index) //위 조건문에 걸리지 않으면(현재 노드가 가장 작으면) 반복문 탈출
                break;

            T temp = heap[index]; //현재 노드 복사
            heap[index] = heap[smallestChildIndex]; //현재 노드 인덱스 자리에 최소값 노드 넣어줌
            heap[smallestChildIndex] = temp; //최소값 노드 자리에 현재 노드 복사본 넣어줌

            index = smallestChildIndex; //현재 노드 인덱스를 최소값 노드의 인덱스로 재설정
        }
        return root;
    }
    public void Clear() //힙 초기화
    {
        heap.Clear();
    }
}
public class MonsterDistCompare : IComparer<NormalMonster> //우선순위 큐의 우선순위 비교 조건을 구현한 실체 클래스
{
    public int Compare(NormalMonster x, NormalMonster y) //x와 y의 플레이어와의 거리를 비교하여 우선순위 설정
    {
        if (x.DistToPlayer < y.DistToPlayer)
        {
            return -1;
        }
        else if(x.DistToPlayer > y.DistToPlayer)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

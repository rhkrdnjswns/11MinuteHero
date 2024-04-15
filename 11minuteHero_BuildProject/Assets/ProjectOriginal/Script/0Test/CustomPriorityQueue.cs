using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomPriorityQueue<T> //Ŀ���͸���¡ �켱���� ť
                                    //Dequeue�� Peek�� �ʿ� ����.
                                    //�ش� �ڷᱸ���� ĳ�̵� ���� ����Ʈ�� �÷��̾���� �Ÿ��� ���� ������ �� �ʱ�ȭ �� ����Ѵ�.
{
    [SerializeField] private List<T> heap = new List<T>(); //�� ������ ����Ʈ
    private IComparer<T> comparer; //����� �������̽� ���� (����� �������̽��� ����ϴ� Ŭ������ ������)

    public T this[int index]
    {
        get
        {
            if (index >= 0 && index < heap.Count) //�ε����� ��ȿ�� ��쿡�� ��ȯ
            {
                return heap[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index is Out of Range."); //��ȿ���� ���� ��� ���� ó��
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
    public CustomPriorityQueue(IComparer<T> comparer) //������
    {
        this.comparer = comparer;
    }

    public void Enqueue(T item) //�ּ� �� ������ ��ť�� �Ͼ
    {
        heap.Add(item);
        int index = heap.Count - 1; //���� ����� �ε����� ������

        while(index > 0)
        {
            int parentIndex = (index - 1) / 2; //�θ� ����� �ε��� ������
            if (comparer.Compare(heap[index], heap[parentIndex]) >= 0) //���� ���� �θ� ����� �켱���� ���� �˻� (��� ������ �ʿ� ���� ��쿡�� �ݺ��� Ż��)
            {
                break;
            }

            T temp = heap[index]; //��� ������ �ʿ��� ����� ó��
            heap[index] = heap[parentIndex];
            heap[parentIndex] = temp;

            index = parentIndex;
        }
    }
    public T Dequeue() //��ť. �ּҰ�(0�� ���)�� ���� ��, ���� �ڸ��� �ٽ� �ּҰ����� ä��� ���� ����. �� ������ �ּҰ� ��� ��ȯ
    {
        if(heap.Count == 0)
        {
            throw new InvalidOperationException("Priority Queue is Empty");
        }

        T root = heap[0]; //��Ʈ ���(�ּҰ�)�� ����
        heap[0] = heap[heap.Count - 1]; //��Ʈ ��� ��ġ�� �� �� ��带 �����Ͽ� �־���
        heap.RemoveAt(heap.Count - 1); //�� �� ��� ���� ����

        int index = 0; //��Ʈ �ε���
        while (true)
        {
            int leftChildIndex = index * 2 + 1; //���� ����� ���� �ڽ� ��� �ε���
            int rightChildIndex = index * 2 + 2; //���� ����� ������ �ڽ� ��� �ε���
            int smallestChildIndex = index; //���� ���� ���� ����� �ε��� (���� �ε����� �ʱ�ȭ)

            if (leftChildIndex < heap.Count && comparer.Compare(heap[leftChildIndex], heap[smallestChildIndex]) < 0) //���� �ڽ� ��尡 ���� ��庸�� ���� ������ 
                smallestChildIndex = leftChildIndex; //�ּҰ� ��带 ����Ű�� �ε����� ���� �ڽ� ��� �ε����� ����

            if (rightChildIndex < heap.Count && comparer.Compare(heap[rightChildIndex], heap[smallestChildIndex]) < 0)//������ �ڽ� ��尡 ���� ��庸�� ���� ������
                smallestChildIndex = rightChildIndex; //�ּҰ� ��带 ����Ű�� �ε����� ������ �ڽ� ��� �ε����� ����

            if (smallestChildIndex == index) //�� ���ǹ��� �ɸ��� ������(���� ��尡 ���� ������) �ݺ��� Ż��
                break;

            T temp = heap[index]; //���� ��� ����
            heap[index] = heap[smallestChildIndex]; //���� ��� �ε��� �ڸ��� �ּҰ� ��� �־���
            heap[smallestChildIndex] = temp; //�ּҰ� ��� �ڸ��� ���� ��� ���纻 �־���

            index = smallestChildIndex; //���� ��� �ε����� �ּҰ� ����� �ε����� �缳��
        }
        return root;
    }
    public void Clear() //�� �ʱ�ȭ
    {
        heap.Clear();
    }
}
public class MonsterDistCompare : IComparer<NormalMonster> //�켱���� ť�� �켱���� �� ������ ������ ��ü Ŭ����
{
    public int Compare(NormalMonster x, NormalMonster y) //x�� y�� �÷��̾���� �Ÿ��� ���Ͽ� �켱���� ����
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

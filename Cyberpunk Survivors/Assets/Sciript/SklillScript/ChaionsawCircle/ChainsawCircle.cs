using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawCircle : MonoBehaviour
{
    private ChainsawOrbit[] chainsaws;
    private int currentIndex = -1;
    private Vector3[] positions = new Vector3[4]
    {
        new Vector3(0, 1, 2),   // Front
        new Vector3(0, 1, -2),  // Back
        new Vector3(2, 1, 0),   // Right
        new Vector3(-2, 1, 0)   // Left
    };

    void Start()
    {
        chainsaws = new ChainsawOrbit[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            chainsaws[i] = transform.GetChild(i).GetComponent<ChainsawOrbit>();
            Debug.Log($"Found chainsaw at index {i}: {transform.GetChild(i).name}");
        }

        // �ʱ⿡ ��� ü�μҿ� ��Ȱ��ȭ
        foreach (var chainsaw in chainsaws)
        {
            if (chainsaw != null)
            {
                chainsaw.Deactivate(); // SetActive(false) ��� Deactivate() ���
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ActivateNextChainsaw();
        }
    }

    public void ActivateNextChainsaw()
    {
        currentIndex++;  // ���� �ε��� ����

        if (currentIndex >= positions.Length || currentIndex >= chainsaws.Length)
        {
            Debug.Log("No more chainsaws to activate");
            currentIndex = positions.Length - 1; 
            return;
        }


        // ���ο� ü�μҿ� Ȱ��ȭ
        if (chainsaws[currentIndex] != null)
        {
            chainsaws[currentIndex].Activate(positions[currentIndex]);
        }
    }
}
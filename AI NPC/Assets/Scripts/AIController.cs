using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class AIController : MonoBehaviour
{
    /*array_text_function_descriptions_ko = [
    "�� �Լ��� NPC�� ��ȭ�ο� ������ �� ȣ���մϴ�.",
    "�� �Լ��� NPC�� �÷��̾�� �ŷ��� �� �� ȣ���մϴ�.",
    "�� �Լ��� NPC�� �÷��̾ �����Ϸ��� �Ҷ� ȣ���մϴ�.",
    "�� �Լ��� NPC�� �÷��̾ ����ٴϷ��� �� �� ȣ���մϴ�."]*/

    [Header("Check & Assign")]
    [SerializeField] private int inputNumber;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerInputField;
    [SerializeField] private GameObject goblinInputField;
    [SerializeField] private GameObject tradePanel;

    [Header("Follow")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float stoppingDistance; // �÷��̾�� ������ �ּ� �Ÿ�

    [Header("Attack")]
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float attackInterval = 3f; // ���� ���� (��)
    private float lastAttackTime = 0f; // ������ ���� �ð�


    private void FixedUpdate()
    {
        switch (inputNumber)
        {
            case 0:
                StayIdle();
                break;
            case 1:
                StartTrade();
                break;
            case 2:
                AttackPlayer();
                break;
            case 3:
                FollowPlayer();
                break;
            default:
                Debug.LogError("IndexOutOfRange: inputNumber�� 0~3���� �����ּ���.");
                break;
        }    
    }


    // 0 : ��ȭ�ο� ����
    public void StayIdle()
    {
        CloseTradeUI();
    }

    // 1 : �÷��̾�� �ŷ��ϴ� ����
    public void StartTrade()
    {
        OpenTradeUI();
    }

    // 2 : �÷��̾ �����ϴ� ����
    public void AttackPlayer()
    {
        CloseTradeUI();

        if (Time.time < lastAttackTime + attackInterval) return;    // ���� ���� üũ
        lastAttackTime = Time.time;                                 // ������ ���� �ð� ����

        if (player == null || stonePrefab == null || throwPoint == null)
        {
            Debug.LogWarning("AttackPlayer() ȣ�� ����");
            return;
        }

        GameObject rock = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - throwPoint.position).normalized;

        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        if (rockRb != null)
        {
            rockRb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }

    // 3 : �÷��̾ ���󰡴� ����
    public void FollowPlayer()
    {
        CloseTradeUI();


        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
            transform.position = newPosition;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
        }
    }


    #region Open / Close UI
    void OpenTradeUI()
    {
        tradePanel.SetActive(true);
    }

    void CloseTradeUI()
    {
        tradePanel.SetActive(false);
    }
    #endregion
}
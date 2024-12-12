using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] private bool isFollowing;
    [SerializeField] private float followSpeed;
    [SerializeField] private float stoppingDistance; // �÷��̾�� ������ �ּ� �Ÿ�


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
        }    
    }


    // 0 : ��ȭ�ο� ����
    public void StayIdle()
    {
        isFollowing = false;

        OpenChatUI();
        CloseTradeUI();
    }

    // 1 : �÷��̾�� �ŷ��ϴ� ����
    public void StartTrade()
    {
        CloseChatUI();
        OpenTradeUI();
    }

    // 2 : �÷��̾ �����ϴ� ����
    public void AttackPlayer()
    {
        CloseChatUI();
        CloseTradeUI();
    }

    // 3 : �÷��̾ ���󰡴� ����
    public void FollowPlayer()
    {
        CloseChatUI();
        CloseTradeUI();

        isFollowing = true;

        if (!isFollowing) return;

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
    void OpenChatUI()
    {
        playerInputField.gameObject.SetActive(true);
        goblinInputField.gameObject.SetActive(true);
    }

    void CloseChatUI()
    {
        playerInputField.gameObject.SetActive(false);
        goblinInputField.gameObject.SetActive(false);
    }

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
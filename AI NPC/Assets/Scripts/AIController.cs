using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    /*array_text_function_descriptions_ko = [
        "�� �Լ��� NPC�� ��ȭ�ο� ������ �� ȣ���մϴ�.",
        "�� �Լ��� NPC�� �÷��̾�� �ŷ��� �� �� ȣ���մϴ�.",
        "�� �Լ��� NPC�� �÷��̾ �����Ϸ��� �Ҷ� ȣ���մϴ�.",
        "�� �Լ��� NPC�� �÷��̾ ����ٴϷ��� �� �� ȣ���մϴ�."]*/

    public enum NPCState
    {
        Peaceful,
        Trading,
        Attacking,
        Following
    }

    [Header("Check & Assign")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerInputField;
    [SerializeField] private GameObject goblinInputField;
    [SerializeField] private GameObject storePanel;

    [Header("Follow")]
    [SerializeField] private bool isFollowing;
    [SerializeField] private float followSpeed;
    [SerializeField] private float stoppingDistance; // �÷��̾�� ������ �ּ� �Ÿ�

    private NPCState currentState;

    private void Start()
    {
        SetState(NPCState.Peaceful); // �⺻ ���� ����
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case NPCState.Peaceful:
                StayIdle();
                break;
            case NPCState.Trading:
                // �ŷ� ���´� FixedUpdate���� ���ٸ� ó���� �ʿ� ����
                break;
            case NPCState.Attacking:
                AttackPlayer();
                break;
            case NPCState.Following:
                FollowPlayer();
                break;
        }
    }

    // ���� ���� �޼���
    public void SetState(object state)
    {
        if (state is NPCState enumState)
        {
            currentState = enumState;
            HandleStateChange(enumState);
        }
        else if (state is int intState && System.Enum.IsDefined(typeof(NPCState), intState))
        {
            currentState = (NPCState)intState;
            HandleStateChange(currentState);
        }
        else if (state is string stringState && System.Enum.TryParse(stringState, out NPCState parsedState))
        {
            currentState = parsedState;
            HandleStateChange(currentState);
        }
        else
        {
            Debug.LogError("Invalid state provided to SetState");
        }
    }

    private void HandleStateChange(NPCState newState)
    {
        switch (newState)
        {
            case NPCState.Peaceful:
                OpenChatUI();
                CloseStoreUI();
                break;
            case NPCState.Trading:
                CloseChatUI();
                OpenStoreUI();
                break;
            case NPCState.Attacking:
                CloseChatUI();
                CloseStoreUI();
                break;
            case NPCState.Following:
                CloseChatUI();
                CloseStoreUI();
                break;
        }
    }

    // 0 : ��ȭ�ο� ����
    public void StayIdle()
    {
        // ��ȭ ������ �ൿ (�ִϸ��̼� �Ǵ� ��Ÿ ���� �߰� ����)
    }

    // 1 : �÷��̾�� �ŷ��ϴ� ����
    public void StartTrade()
    {
        // �ŷ� ���¿��� ������ FixedUpdate ó�� ���� UI ������
    }

    // 2 : �÷��̾ �����ϴ� ����
    public void AttackPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * followSpeed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);

        // �÷��̾���� �Ÿ� ��� �� ���� �ִϸ��̼� Ʈ���� �߰� ����
    }

    // 3 : �÷��̾ ���󰡴� ����
    public void FollowPlayer()
    {
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
    private void OpenChatUI()
    {
        playerInputField.gameObject.SetActive(true);
        goblinInputField.gameObject.SetActive(true);
    }

    private void CloseChatUI()
    {
        playerInputField.gameObject.SetActive(false);
        goblinInputField.gameObject.SetActive(false);
    }

    private void OpenStoreUI()
    {
        storePanel.SetActive(true);
    }

    private void CloseStoreUI()
    {
        storePanel.SetActive(false);
    }
    #endregion
}

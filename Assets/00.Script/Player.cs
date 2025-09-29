using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator headAnim;
    public Animator bodyAnim;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;

    [Header("�߻� ��Ÿ��")]
    [SerializeField] private float fireCooldown = 0.3f; // �߻� ���� (��)
    private float lastFireTime = -999f;

    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;

    private bool isHeadLocked = false; // �Ӹ� �ִϸ��̼� ��� ����
    private Coroutine headLockRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.bodyType != RigidbodyType2D.Dynamic)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        // �̵� �Է� (WASD)
        moveInput.x = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        moveInput.y = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        moveInput = moveInput.normalized;

        // �̵� �ִϸ��̼� (�Ӹ� ��� ���� ���� ���� ����)
        if (!isHeadLocked)
        {
            UpdateAnimations(moveInput);
        }
        else
        {
            UpdateBodyOnly(moveInput);
        }

        // �߻� �Է� (����Ű)
        Vector2 shootDir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) shootDir = Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow)) shootDir = Vector2.down;
        else if (Input.GetKey(KeyCode.LeftArrow)) shootDir = Vector2.left;
        else if (Input.GetKey(KeyCode.RightArrow)) shootDir = Vector2.right;

        if (shootDir != Vector2.zero && CanFire())
        {
            Fire(shootDir);

            // �Ӹ� �߻� ���� ���� (0.5�� ����)
            if (headLockRoutine != null) StopCoroutine(headLockRoutine);
            headLockRoutine = StartCoroutine(LockHeadDirection(shootDir, 0.5f));
        }
    }

    private void FixedUpdate()
    {
        // �ε巯�� �̵�
        Vector2 targetVelocity = moveInput * moveSpeed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
    }

    private void UpdateAnimations(Vector2 dir)
    {
        if (dir.magnitude > 0.1f)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    headAnim.Play("RightAnim");
                    bodyAnim.Play("RightBody");
                }
                else
                {
                    headAnim.Play("LeftAnim");
                    bodyAnim.Play("LeftBody");
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    headAnim.Play("BehindAnim");
                    bodyAnim.Play("BehindBody");
                }
                else
                {
                    headAnim.Play("FrontHead");
                    bodyAnim.Play("FrontBody");
                }
            }
        }
        else
        {
            headAnim.Play("FrontHead");
            bodyAnim.Play("Idle");
        }
    }

    // ���� ���� (�Ӹ��� ��� ������ ��)
    private void UpdateBodyOnly(Vector2 dir)
    {
        if (dir.magnitude > 0.1f)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                bodyAnim.Play(dir.x > 0 ? "RightBody" : "LeftBody");
            }
            else
            {
                bodyAnim.Play(dir.y > 0 ? "BehindBody" : "FrontBody");
            }
        }
        else
        {
            bodyAnim.Play("Idle");
        }
    }

    private void UpdateHeadShootAnim(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
                headAnim.Play("RightAnim");
            else
                headAnim.Play("LeftAnim");
        }
        else
        {
            if (dir.y > 0)
                headAnim.Play("BehindAnim");
            else
                headAnim.Play("FrontHead");
        }
    }

    private IEnumerator LockHeadDirection(Vector2 dir, float duration)
    {
        isHeadLocked = true;
        UpdateHeadShootAnim(dir); // �Ӹ��� �߻� �������� ȸ��
        yield return new WaitForSeconds(duration);
        isHeadLocked = false; // �ٽ� �̵� �ִϸ��̼����� ����
    }

    private void Fire(Vector2 shootDir)
    {
        // �߻� �ð� ����
        lastFireTime = Time.time;

        // �Ѿ� ����
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDir.normalized * bulletSpeed;

        // �Ѿ� ȸ��
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private bool CanFire()
    {
        return Time.time >= lastFireTime + fireCooldown;
    }
}

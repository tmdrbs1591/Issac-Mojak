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

    [Header("발사 쿨타임")]
    [SerializeField] private float fireCooldown = 0.3f; // 발사 간격 (초)
    private float lastFireTime = -999f;

    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;

    private bool isHeadLocked = false; // 머리 애니메이션 잠금 여부
    private Coroutine headLockRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.bodyType != RigidbodyType2D.Dynamic)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        // 이동 입력 (WASD)
        moveInput.x = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        moveInput.y = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        moveInput = moveInput.normalized;

        // 이동 애니메이션 (머리 잠금 중일 때는 몸만 갱신)
        if (!isHeadLocked)
        {
            UpdateAnimations(moveInput);
        }
        else
        {
            UpdateBodyOnly(moveInput);
        }

        // 발사 입력 (방향키)
        Vector2 shootDir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) shootDir = Vector2.up;
        else if (Input.GetKey(KeyCode.DownArrow)) shootDir = Vector2.down;
        else if (Input.GetKey(KeyCode.LeftArrow)) shootDir = Vector2.left;
        else if (Input.GetKey(KeyCode.RightArrow)) shootDir = Vector2.right;

        if (shootDir != Vector2.zero && CanFire())
        {
            Fire(shootDir);

            // 머리 발사 방향 고정 (0.5초 유지)
            if (headLockRoutine != null) StopCoroutine(headLockRoutine);
            headLockRoutine = StartCoroutine(LockHeadDirection(shootDir, 0.5f));
        }
    }

    private void FixedUpdate()
    {
        // 부드러운 이동
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

    // 몸만 갱신 (머리는 잠금 상태일 때)
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
        UpdateHeadShootAnim(dir); // 머리를 발사 방향으로 회전
        yield return new WaitForSeconds(duration);
        isHeadLocked = false; // 다시 이동 애니메이션으로 복귀
    }

    private void Fire(Vector2 shootDir)
    {
        // 발사 시간 갱신
        lastFireTime = Time.time;

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDir.normalized * bulletSpeed;

        // 총알 회전
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private bool CanFire()
    {
        return Time.time >= lastFireTime + fireCooldown;
    }
}

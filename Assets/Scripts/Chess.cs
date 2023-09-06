using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public static Object Instance;

    public DrawWithMouse drawControl;

    public float speed = 10f;

    private Vector3 startPos;

    bool startMovement = false;

    Vector3[] positions;

    int moveIndex = 0;

    private float min_X = -2.15f;

    private float max_X = 2.15f;

    private float min_Y = -4.85f;

    private float max_Y = 4.85f;

    public GameObject vfxOnDeath;

    public GameObject vfxSuccess;

    public Sprite sprite;

    private new SpriteRenderer renderer;

    public Color colorToChange;

    private void Awake()
    {
        Instance = this;

        startPos = transform.position;

        renderer = GetComponentInChildren<SpriteRenderer>();

        renderer.sprite = sprite;
    }

    private void OnMouseDown()
    {
        drawControl.StartLine(transform.position);
    }

    private void OnMouseDrag()
    {
        drawControl.Updateline();
    }

    private void OnMouseUp()
    {
        positions = new Vector3[drawControl.line.positionCount];
        drawControl.line.GetPositions(positions);
        drawControl.ResetLine();
        startMovement = true;
        moveIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject vfx = Instantiate(vfxSuccess, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);

            sprite = collision.gameObject.GetComponent<Enemy>().sprite;

            renderer.sprite = sprite;

            renderer.color = colorToChange;

            collision.gameObject.SetActive(false);

            StartCoroutine(RemoveEnemyFromList(collision.gameObject.GetComponent<Enemy>()));
        }
    }

    private IEnumerator RemoveEnemyFromList(Enemy enemy)
    {
        yield return new WaitForSeconds(1f);

        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].enemies.Remove(enemy);
        //gameObject.SetActive(false);
    }

    public void ReSetPos()
    {
        transform.position = startPos;
        startMovement = false;
        drawControl.ResetLine();
    }

    private void Update()
    {
        if (startMovement)
        {
            //CheckPos();

            Vector2 currentPos = positions[moveIndex];
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);

            float distance = Vector2.Distance(currentPos, transform.position);
            if (distance <= 0.05f)
            {
                moveIndex++;
            }

            if (moveIndex > positions.Length - 1)
            {
                startMovement = false;
            }
        }
    }

    private void CheckPos()
    {
        if (transform.position.x < min_X)
        {
            Vector3 moveDirX = new Vector3(min_X, transform.position.y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X)
        {
            Vector3 moveDirX = new Vector3(max_X, transform.position.y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(transform.position.x, min_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(transform.position.x, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x < min_X && transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(min_X, min_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x < min_X && transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(min_X, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X && transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(max_X, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X && transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(max_X, min_Y, 0f);
            transform.position = moveDirX;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PolygonColliderUpdater : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    private SpriteRenderer spriteRenderer;
    private Sprite currentSprite;

    void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (spriteRenderer.sprite == null)
            return;

        if (currentSprite != spriteRenderer.sprite)
        {
            currentSprite = spriteRenderer.sprite;
            UpdateCollider();
        }
    }

    void UpdateCollider()
    {
        polyCollider.pathCount = currentSprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();

        for (int i = 0; i < polyCollider.pathCount; i++)
        {
            path.Clear();
            currentSprite.GetPhysicsShape(i, path);

            // Kalau sprite-nya di-flip secara horizontal, balik posisi titik collider-nya
            if (spriteRenderer.flipX)
            {
                for (int j = 0; j < path.Count; j++)
                {
                    path[j] = new Vector2(-path[j].x, path[j].y);
                }
            }

            polyCollider.SetPath(i, path.ToArray());
        }
    }
}

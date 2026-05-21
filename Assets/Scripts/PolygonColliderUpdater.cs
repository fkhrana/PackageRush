using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PolygonColliderUpdater : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    private SpriteRenderer spriteRenderer;
    private Sprite currentSprite;
    private bool lastFlipX;
    private readonly List<Vector2> physicsShape = new List<Vector2>();

    private void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (spriteRenderer.sprite == null)
            return;

        if (currentSprite != spriteRenderer.sprite || lastFlipX != spriteRenderer.flipX)
        {
            currentSprite = spriteRenderer.sprite;
            lastFlipX = spriteRenderer.flipX;
            UpdateCollider();
        }
    }

    private void UpdateCollider()
    {
        polyCollider.pathCount = currentSprite.GetPhysicsShapeCount();

        for (int i = 0; i < polyCollider.pathCount; i++)
        {
            physicsShape.Clear();
            currentSprite.GetPhysicsShape(i, physicsShape);

            if (spriteRenderer.flipX)
            {
                for (int j = 0; j < physicsShape.Count; j++)
                {
                    physicsShape[j] = new Vector2(-physicsShape[j].x, physicsShape[j].y);
                }
            }

            polyCollider.SetPath(i, physicsShape.ToArray());
        }
    }
}

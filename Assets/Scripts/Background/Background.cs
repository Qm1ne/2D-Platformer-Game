using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier = new Vector2(0.5f, 0.5f);
    [SerializeField] private bool infiniteHorizontal = true;
    [SerializeField] private bool infiniteVertical = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        
        Sprite sprite = GetComponent<SpriteRenderer>()?.sprite;
        if (sprite != null)
        {
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        transform.position += new Vector3(
            deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y,
            0
        );
        lastCameraPosition = cameraTransform.position;

        if (infiniteHorizontal)
        {
            float distanceFromCamera = cameraTransform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromCamera) >= textureUnitSizeX)
            {
                float offsetPositionX = (distanceFromCamera > 0) ? textureUnitSizeX : -textureUnitSizeX;
                transform.position = new Vector3(
                    transform.position.x + offsetPositionX,
                    transform.position.y,
                    transform.position.z
                );
            }
        }

        if (infiniteVertical)
        {
            float distanceFromCamera = cameraTransform.position.y - transform.position.y;
            if (Mathf.Abs(distanceFromCamera) >= textureUnitSizeY)
            {
                float offsetPositionY = (distanceFromCamera > 0) ? textureUnitSizeY : -textureUnitSizeY;
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + offsetPositionY,
                    transform.position.z
                );
            }
        }
    }

}

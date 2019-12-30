using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class BattleObjectCollider2D : BattleComponent
{
    public LayerMask collisionMask;

    public const float skinWidth = .15f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public CollisionInfo collisions;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    private BoxCollider collider;
    private RaycastOrigins raycastOrigins;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity){
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0) HorizontalCollisions(ref velocity);
        if (velocity.y != 0) VerticalCollisions(ref velocity);
        
        transform.Translate(velocity);
    }

    private void VerticalCollisions(ref Vector3 velocity){
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        
        for (int i=0; i < verticalRayCount; i++){
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right*(verticalRaySpacing*i + velocity.x);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin,Vector2.up * directionY, out hit, rayLength, collisionMask)){
                Platform otherPlat = hit.transform.GetComponent<Platform>();

                //If it's a platform, we need to check if we're supposed to be "phasing" through it. If we are, skip this check
                if (otherPlat){
                    //If we're going up and it's a pass through
                    bool upPhase = (directionY == 1 && otherPlat.passThrough);
                    //or if we're going down, it's a fall through, and we're currently phasing
                    bool downPhase = (directionY == -1 && otherPlat.fallThrough && GetBoolVar(TussleConstants.ColliderVariableNames.IS_PHASING));

                    if ( upPhase || downPhase ){
                        continue;
                    }
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    private void HorizontalCollisions(ref Vector3 velocity){
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        
        for (int i=0; i < horizontalRayCount; i++){
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up*(horizontalRaySpacing*i);
            
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin,Vector2.right * directionX, out hit, rayLength, collisionMask)){
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void OnLanding(){

    }

    void CheckForGround(){
        SetVar(TussleConstants.FighterVariableNames.IS_GROUNDED, collisions.below);
        // float rayLength = skinWidth*2;

        // for (int i=0; i < verticalRayCount; i++){
        //     Vector2 rayOrigin = raycastOrigins.bottomLeft;
        //     rayOrigin += Vector2.right*(verticalRaySpacing*i);
        //     RaycastHit hit;

        //     if (Physics.Raycast(rayOrigin,Vector2.down, out hit, rayLength, collisionMask)){
        //         SetVar(TussleConstants.FighterVariableNames.IS_GROUNDED, true);
        //     }
        // }
    }

    private void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x,bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x,bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x,bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x,bounds.max.y);
    }

    void CalculateRaySpacing(){
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount,2,int.MaxValue);
        verticalRayCount = Mathf.Clamp(horizontalRayCount,2,int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above,below;
        public bool left,right;

        public void Reset(){
            above = below = false;
            left = right = false;
        }
    }
}

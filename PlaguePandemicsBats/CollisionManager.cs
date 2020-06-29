using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PlaguePandemicsBats
{
    public class CollisionManager
    {
        private List<Collider> colliders;
        private List<Collider> inRangeColliders;
        private Game1 _game;

        public CollisionManager(Game1 game)
        {
            _game = game;

            colliders = new List<Collider>();
            inRangeColliders = new List<Collider>();
        }

        public void Add(Collider c)
        {
            colliders.Add(c);
        }

        public void Remove(Collider c)
        {
            colliders.Remove(c);
        }

        public void Update(GameTime gameTime)
        {
            // In the beginning of each frame, no one collides!
            foreach (Collider collider in colliders)
            {
                collider.EmptyCollisions();
            }

            inRangeColliders = colliders.Where(c => Vector2.Distance(c._position, _game.Player.Position) <= 6.2f).ToList();

            // Check collisions for each collider combination
            for (int i = 0; i < inRangeColliders.Count - 1; i++)
            {
                for (int j = i + 1; j < inRangeColliders.Count; j++)
                {
                    bool areColliding = false;
                    if (inRangeColliders[i] is CircleCollider c1 && inRangeColliders[j] is CircleCollider c2)
                    {
                        areColliding = CircleVsCircle(c1, c2);
                    } 
                    else if (inRangeColliders[i] is AABBCollider a1 && inRangeColliders[j] is AABBCollider a2)
                    {
                        areColliding = AabbVsAabb(a1, a2);
                    }
                    else if (inRangeColliders[i] is AABBCollider a3 && inRangeColliders[j] is CircleCollider c3)
                    {
                        areColliding = CircleVsAabb(c3, a3);
                    }
                    else if (inRangeColliders[i] is CircleCollider c4 && inRangeColliders[j] is AABBCollider a4)
                    {
                        areColliding = CircleVsAabb(c4, a4);
                    }
                    else if (inRangeColliders[i] is OBBCollider o1 && inRangeColliders[j] is OBBCollider o2)
                    {
                        areColliding = ObbVsObb(o1, o2);
                    }
                    else if (inRangeColliders[i] is OBBCollider o3 && inRangeColliders[j] is CircleCollider c5)
                    {
                        areColliding = ObbVsCircle(o3, c5);
                    }
                    else if (inRangeColliders[i] is CircleCollider c6 && inRangeColliders[j] is OBBCollider o4)
                    {
                        areColliding = ObbVsCircle(o4, c6);
                    }
                    else if (inRangeColliders[i] is OBBCollider o5 && inRangeColliders[j] is AABBCollider a5)
                    {
                        areColliding = ObbVsAabb(o5, a5);
                    }
                    else if (inRangeColliders[i] is AABBCollider a6 && inRangeColliders[j] is OBBCollider o6)
                    {
                        areColliding = ObbVsAabb(o6, a6);
                    }
                    else
                    {
                        Type t1 = inRangeColliders[i].GetType();
                        Type t2 = inRangeColliders[j].GetType();
                        throw new Exception($"No collision function defined for types {t1} and {t2}");
                    }

                    if (areColliding)
                    {
                        // colliders[i]._inCollision = true;
                        // colliders[j]._inCollision = true;
                        inRangeColliders[i].SetCollision(inRangeColliders[j]);
                        inRangeColliders[j].SetCollision(inRangeColliders[i]);
                    }
                }
            }
        }
        
        bool CircleVsCircle(CircleCollider c1, CircleCollider c2)
        {
            return (c1._position - c2._position).Length() <= c1._radius + c2._radius;
        }

        bool AabbVsAabb(AABBCollider c1, AABBCollider c2)
        {
            return c1._rectangle.Intersects(c2._rectangle);
        }

        bool CircleVsAabb(CircleCollider c1, AABBCollider c2)
        {  
            bool collision = c2._rectangle.Contains(c1._position);
            // horizontal rectangle lines
            for (int x = c2._rectangle.Left; !collision && x <= c2._rectangle.Right; x++)
            {
                if ((c1._position - new Vector2(x, c2._rectangle.Top)).Length() < c1._radius)
                {
                    collision = true;
                }

                // similar to above code, just more compact
                collision = collision || (c1._position - new Vector2(x, c2._rectangle.Bottom)).Length() < c1._radius;

            }
            // vertical rectangle lines
            for (int y = c2._rectangle.Top; !collision && y <= c2._rectangle.Bottom; y++)
            {
                if ((c1._position - new Vector2(c2._rectangle.Left, y)).Length() < c1._radius)
                {
                    collision = true;
                }
                if ((c1._position - new Vector2(c2._rectangle.Right, y)).Length() < c1._radius)
                {
                    collision = true;
                }
            }
   
            return collision;
        }

        bool ObbVsObb(OBBCollider a, OBBCollider b)
        {
            float ra, rb;
            float[,] R, AbsR;
            R = new float[2, 2];
            AbsR = new float[2, 2];
            
            // Compute rotation matrix expressing b in a’s coordinate frame
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    // u is the orientation u[0] = _orientation1  [1] = _orientation2
                    R[i,j] = Vector2.Dot( i == 0 ? a._orientation1 : a._orientation2,
                                          j == 0 ? b._orientation1 : b._orientation2);
            }
            
            // Compute translation vector t
            Vector2 t = b._position - a._position; 
            // Bring translation into a’s coordinate frame
            t = new Vector2( Vector2.Dot(t, a._orientation1),
                             Vector2.Dot(t, a._orientation2));

            // Compute common subexpressions. Add in an epsilon term to
            // // counteract arithmetic errors when two edges are parallel and
            // // their cross product is (near) null (see text for details)
            const float EPSILON = 0.00001f; 
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    AbsR[i,j] = Math.Abs(R[i,j]) + EPSILON;
            }
            
            // Test axes L = A0, L = A1, L = A2
            for (int i = 0; i < 2; i++) {
                // e == extends // e[0] == extends.X // e[1] == extends.Y
                // ra = a.e[i];
                ra = a._extends.Pos(i);
                // rb = b.e[0] * AbsR[i][0] + b.e[1] * AbsR[i][1] + b.e[2] * AbsR[i][2];
                rb = b._extends.Pos(0) * AbsR[i,0] + b._extends.Pos(1) * AbsR[i,1];
                if (Math.Abs(t.Pos(i)) > ra + rb) return false;
            }
            
            // Test axes L = B0, L = B1, L = B2
            for (int i = 0; i < 2; i++) {
                ra = a._extends.Pos(0) * AbsR[0,i] + a._extends.Pos(1) * AbsR[1,i];
                rb = b._extends.Pos(i);
                if (Math.Abs(t.Pos(0) * R[0,i] + t.Pos(1) * R[1,i]) > ra + rb) return false;
            }
            
            return true;
        }

        bool ObbVsCircle(OBBCollider box, CircleCollider circle)
        {
            // Find point p on OBB closest to sphere center
            Vector2 p = ClosestPtPointOBB(circle._position, box); 
            // Sphere and OBB intersect if the (squared) distance from sphere
            // center to point p is less than the (squared) sphere radius
            Vector2 v = p - circle._position;
            return Vector2.Dot(v, v) <= circle._radius * circle._radius;
        }

        private Vector2 ClosestPtPointOBB(Vector2 p, OBBCollider b)
        {
            Vector2 d = p - b._position; 
            // Start result at center of box; make steps from there
            Vector2 q = b._position; 
            // For each OBB axis...
            for (int i = 0; i < 2; i++) { 
                // ...project d onto that axis to get the distance
                // along the axis of d from the box center
                float dist = Vector2.Dot(d, i == 0 ? b._orientation1 : b._orientation2); 
                // If distance farther than the box extents, clamp to the box
                if (dist > b._extends.Pos(i)) dist = b._extends.Pos(i);
                if (dist < -b._extends.Pos(i)) dist = -b._extends.Pos(i); 
                // Step that distance along the axis to get world coordinate
                q += dist * (i == 0 ? b._orientation1 : b._orientation2);
            }

            return q;
        }

        bool ObbVsAabb(OBBCollider o, AABBCollider a)
        {
            return ObbVsObb(o, a.ToObb());
        }
    }
}
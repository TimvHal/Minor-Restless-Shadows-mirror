// A C# program to check if a given point  
// lies inside a given polygon 
// Refer https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/ 
// for explanation of functions onSegment(), 
// orientation() and doIntersect()  
using System;
using UnityEngine;

public static class Polygons
{

    // Define Infinite (Using INT_MAX  
    // caused overflow problems) 
    static int INF = 10000;
    

    // Given three colinear points p, q, r,  
    // the function checks if point q lies 
    // on line segment 'pr' 
    static bool onSegment(Vector2 p, Vector2 q, Vector2 r)
    {
        if (q.x <= Math.Max(p.x, r.x) &&
            q.x >= Math.Min(p.x, r.x) &&
            q.y <= Math.Max(p.y, r.y) &&
            q.y >= Math.Min(p.y, r.y))
        {
            return true;
        }

        return false;
    }

    // To find orientation of ordered triplet (p, q, r). 
    // The function returns following values 
    // 0 --> p, q and r are colinear 
    // 1 --> Clockwise 
    // 2 --> Counterclockwise 
    static int orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        int val = (int) ((q.y - p.y) * (r.x - q.x) -
                         (q.x - p.x) * (r.y - q.y));

        if (val == 0)
        {
            return 0; // colinear 
        }

        return (val > 0) ? 1 : 2; // clock or counterclock wise 
    }

    // The function that returns true if  
    // line segment 'p1q1' and 'p2q2' intersect. 
    static bool doIntersect(Vector2 p1, Vector2 q1,
        Vector2 p2, Vector2 q2)
    {
        // Find the four orientations needed for  
        // general and special cases 
        int o1 = orientation(p1, q1, p2);
        int o2 = orientation(p1, q1, q2);
        int o3 = orientation(p2, q2, p1);
        int o4 = orientation(p2, q2, q1);

        // General case 
        if (o1 != o2 && o3 != o4)
        {
            return true;
        }

        // Special Cases 
        // p1, q1 and p2 are colinear and 
        // p2 lies on segment p1q1 
        if (o1 == 0 && onSegment(p1, p2, q1))
        {
            return true;
        }

        // p1, q1 and p2 are colinear and 
        // q2 lies on segment p1q1 
        if (o2 == 0 && onSegment(p1, q2, q1))
        {
            return true;
        }

        // p2, q2 and p1 are colinear and 
        // p1 lies on segment p2q2 
        if (o3 == 0 && onSegment(p2, p1, q2))
        {
            return true;
        }

        // p2, q2 and q1 are colinear and 
        // q1 lies on segment p2q2 
        if (o4 == 0 && onSegment(p2, q1, q2))
        {
            return true;
        }

        // Doesn't fall in any of the above cases 
        return false;
    }

    // Returns true if the point p lies  
    // inside the polygon[] with n vertices 
    static bool isInside(Vector2[] polygon, int n, Vector2 p)
    {
        // There must be at least 3 vertices in polygon[] 
        if (n < 3)
        {
            return false;
        }

        // Create a point for line segment from p to infinite 
        Vector2 extreme = new Vector2(INF, p.y);

        // Count intersections of the above line  
        // with sides of polygon 
        int count = 0, i = 0;
        do
        {
            int next = (i + 1) % n;

            // Check if the line segment from 'p' to  
            // 'extreme' intersects with the line  
            // segment from 'polygon[i]' to 'polygon[next]' 
            if (doIntersect(polygon[i],
                polygon[next], p, extreme))
            {
                // If the point 'p' is colinear with line  
                // segment 'i-next', then check if it lies  
                // on segment. If it lies, return true, otherwise false 
                if (orientation(polygon[i], p, polygon[next]) == 0)
                {
                    return onSegment(polygon[i], p,
                        polygon[next]);
                }

                count++;
            }

            i = next;
        } while (i != 0);

        // Return true if count is odd, false otherwise 
        return (count % 2 == 1); // Same as (count%2 == 1) 
    }

    public static bool calculateInside(Vector2[] polygon1, Vector2 target)
    {
        int n = polygon1.Length;
        return isInside(polygon1, n, target);
    }
}
  
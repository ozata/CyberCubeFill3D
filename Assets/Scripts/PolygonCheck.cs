using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code taken from
// https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/#:~:text=1)%20Draw%20a%20horizontal%20line,true%2C%20then%20point%20lies%20outside.


// A C# program to check if a given point
// lies inside a given polygon
// Refer https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
// for explanation of functions onSegment(),
// orientation() and doIntersect()
using System;

public class PolygonCheck: MonoBehaviour {

  // Define Infinite (Using INT_MAX
  // caused overflow problems)
  static float INF = 100000;

  public class Point {
    public float x;
    public float z;

    public Point(float x, float z) {
      this.x = x;
      this.z = z;
    }
  };


  void Start(){
    Point player = new Point(-3.5F,-1.5F);
    Point[] acun = {new Point(0F, -1.5F),
                            new Point(-3.5F, -1.5F),
                            new Point(-3.5F, 7.5F),
                            new Point(3.5F, 7.5F),
                            new Point(3.5F, -1.5F),
                            new Point(4.5F, 2F),
                            };


    print("Player is inside polygon: " + isInside(acun, 5, player));
  }

  void Update() {

  }

  // Given three colinear points p, q, r,
  // the function checks if point q lies
  // on line segment 'pr'
  static bool onSegment(Point p, Point q, Point r) {
    if (q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) && q.z <= Math.Max(p.z, r.z) && q.z >= Math.Min(p.z, r.z)) {
      return true;
    }
    return false;
  }

  // To find orientation of ordered triplet (p, q, r).
  // The function returns following values
  // 0 --> p, q and r are colinear
  // 1 --> Clockwise
  // 2 --> Counterclockwise
  static int orientation(Point p, Point q, Point r) {
    float val = (q.z - p.z) * (r.x - q.x) - (q.x - p.x) * (r.z - q.z);

    if (val == 0) {
      return 0; // colinear
    }
    return (val > 0) ? 1 : 2; // clock or counterclock wise
  }

  // The function that returns true if
  // line segment 'p1q1' and 'p2q2' intersect.
  static bool doIntersect(Point p1, Point q1, Point p2, Point q2) {
    // Find the four orientations needed for
    // general and special cases
    int o1 = orientation(p1, q1, p2);
    int o2 = orientation(p1, q1, q2);
    int o3 = orientation(p2, q2, p1);
    int o4 = orientation(p2, q2, q1);

    // General case
    if (o1 != o2 && o3 != o4) {
      return true;
    }

    // Special Cases
    // p1, q1 and p2 are colinear and
    // p2 lies on segment p1q1
    if (o1 == 0 && onSegment(p1, p2, q1)) {
      return true;
    }

    // p1, q1 and p2 are colinear and
    // q2 lies on segment p1q1
    if (o2 == 0 && onSegment(p1, q2, q1)) {
      return true;
    }

    // p2, q2 and p1 are colinear and
    // p1 lies on segment p2q2
    if (o3 == 0 && onSegment(p2, p1, q2)) {
      return true;
    }

    // p2, q2 and q1 are colinear and
    // q1 lies on segment p2q2
    if (o4 == 0 && onSegment(p2, q1, q2)) {
      return true;
    }

    // Doesn't fall in any of the above cases
    return false;
  }

  // Returns true if the point p lies
  // inside the polygon[] with n vertices
  public static bool isInside(Point[] polygon, int n, Point p) {
    // There must be at least 3 vertices in polygon[]
    if (n < 3) {
      return false;
    }

    // Create a point for line segment from p to infinite
    Point extreme = new Point(INF, p.z);

    // Count intersections of the above line
    // with sides of polygon
    int count = 0,
    i = 0;
    do {
      int next = (i + 1) % n;

      // Check if the line segment from 'p' to
      // 'extreme' intersects with the line
      // segment from 'polygon[i]' to 'polygon[next]'
      if (doIntersect(polygon[i], polygon[next], p, extreme)) {
        // If the point 'p' is colinear with line
        // segment 'i-next', then check if it lies
        // on segment. If it lies, return true, otherwise false
        if (orientation(polygon[i], p, polygon[next]) == 0) {
          return onSegment(polygon[i], p, polygon[next]);
        }
        count++;
      }
      i = next;
    } while ( i != 0 );

    // Return true if count is odd, false otherwise
    return (count % 2 == 1); // Same as (count%2 == 1)
  }


}
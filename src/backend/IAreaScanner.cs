using System.Collections.Generic;

interface IAreaScanner
{
    // returns all people's locations in the given circle
    List<Vector> getAllVectorsInRange(Vector point, float radius);
}

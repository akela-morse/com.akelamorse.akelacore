using UnityEngine;

namespace Akela.Tools
{
    public static class RaycastHitExtensions
    {
        public static void InterpolateNormalIfNecessary(this ref RaycastHit hit)
        {
            if (hit.collider is not MeshCollider mc || !mc.sharedMesh || !mc.sharedMesh.isReadable || mc.convex)
                return;

            var mesh = mc.sharedMesh;
            var meshNormals = mesh.normals;
            var meshTris = mesh.triangles;

            var n0 = meshNormals[meshTris[hit.triangleIndex * 3 + 0]];
            var n1 = meshNormals[meshTris[hit.triangleIndex * 3 + 1]];
            var n2 = meshNormals[meshTris[hit.triangleIndex * 3 + 2]];

            var interpolatedNormal = n0 * hit.barycentricCoordinate.x + n1 * hit.barycentricCoordinate.y + n2 * hit.barycentricCoordinate.z;

            hit.normal = hit.transform.TransformDirection(interpolatedNormal.normalized);
        }
    }
}
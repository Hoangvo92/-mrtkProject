// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.MixedReality.Toolkit.Physics
{
    /// <summary>
    /// Compares the Raycast Results from Unity's Graphic &amp; Physics Raycasters.
    /// </summary>
    public class RaycastResultComparer : IComparer<ComparableRaycastResult>
    {
        private static readonly List<Func<ComparableRaycastResult, ComparableRaycastResult, int>> comparers = new List<Func<ComparableRaycastResult, ComparableRaycastResult, int>>
        {
            CompareRaycastsByLayerMaskPrioritization,
            CompareRaycastsBySortingLayer,
            CompareRaycastsBySortingOrder,
            CompareRaycastsByCanvasDepth,
            CompareRaycastsByDistance,
        };

        protected virtual List<Func<ComparableRaycastResult, ComparableRaycastResult, int>> Comparers
        {
            get
            {
                return comparers;
            }
        }

        public int Compare(ComparableRaycastResult left, ComparableRaycastResult right)
        {
            for (var i = 0; i < Comparers.Count; i++)
            {
                var result = Comparers[i](left, right);
                if (result != 0)
                {
                    return result;
                }
            }
            return 0;
        }

        protected static int CompareRaycastsByLayerMaskPrioritization(ComparableRaycastResult left, ComparableRaycastResult right)
        {
            // Lower is better, -1 is not relevant.
            return right.LayerMaskIndex.CompareTo(left.LayerMaskIndex);
        }

        protected static int CompareRaycastsBySortingLayer(ComparableRaycastResult left, ComparableRaycastResult right)
        {
            // Higher is better.
            return left.RaycastResult.sortingLayer.CompareTo(right.RaycastResult.sortingLayer);
        }

        protected static int CompareRaycastsBySortingOrder(ComparableRaycastResult left, ComparableRaycastResult right)
        {
            // Higher is better.
            return left.RaycastResult.sortingOrder.CompareTo(right.RaycastResult.sortingOrder);
        }

        protected static int CompareRaycastsByCanvasDepth(ComparableRaycastResult left, ComparableRaycastResult right)
        {
            // Module is the graphic raycaster on the canvases.
            if (left.RaycastResult.module.transform.IsParentOrChildOf(right.RaycastResult.module.transform))
            {
                // Higher is better.
                return left.RaycastResult.depth.CompareTo(right.RaycastResult.depth);
            }
            return 0;
        }

        protected static int CompareRaycastsByDistance(ComparableRaycastResult left, ComparableRaycastResult right)
        {
            // Lower is better.
            return right.RaycastResult.distance.CompareTo(left.RaycastResult.distance);
        }
    }
}

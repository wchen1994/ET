using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Scene))]
	public class OperaComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public Vector3 ClickPoint;
        public float3 MoveDir;

	    public int mapMask;
        public ETCancellationToken cancellationToken;
    }
}

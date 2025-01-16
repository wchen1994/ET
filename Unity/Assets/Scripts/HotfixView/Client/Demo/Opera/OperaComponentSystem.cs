using System.Threading;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{

    [EntitySystemOf(typeof(OperaComponent))]
    [FriendOf(typeof(OperaComponent))]
    public static partial class OperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
            self.mapMask = LayerMask.GetMask("Map");
            self.cancellationToken = new ETCancellationToken();

            self.MoveDirUpdate().Coroutine();
        }

        [EntitySystem]
        private static void Destroy(this OperaComponent self)
        {
            self.cancellationToken?.Cancel();
        }

        [EntitySystem]
        private static void Update(this OperaComponent self)
        {
            // 热重载
            if (Input.GetKeyDown(KeyCode.R))
            {
                CodeLoader.Instance.Reload();
                return;
            }

            // 寻路
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
                {
                    C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
                    c2MPathfindingResult.Position = hit.point;
                    self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
                }
            }

            // 传送
            if (Input.GetKeyDown(KeyCode.T))
            {
                C2M_TransferMap c2MTransferMap = C2M_TransferMap.Create();
                self.Root().GetComponent<ClientSenderComponent>().Call(c2MTransferMap).Coroutine();
            }
        }

        private static async ETTask MoveDirUpdate(this OperaComponent self)
        {
            while (!self.IsDisposed)
            {
                float3 moveDir = float3.zero;
                moveDir.x = Input.GetAxisRaw("Horizontal");
                moveDir.z = Input.GetAxisRaw("Vertical");

                var equal = moveDir == self.MoveDir;
                if (!equal.x || !equal.y || !equal.z)
                {
                    C2M_MoveDir c2MMoveDir = C2M_MoveDir.Create();
                    c2MMoveDir.Direction = moveDir;
                    self.Root().GetComponent<ClientSenderComponent>().Send(c2MMoveDir);

                    self.MoveDir = moveDir;
                }

                int fps = 25;
                int ms = 1000 / fps;
                await self.Root().GetComponent<TimerComponent>().WaitAsync(ms, self.cancellationToken);
            }
        }
    }
}
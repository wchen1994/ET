using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_MoveDirHandler : MessageLocationHandler<Unit, C2M_MoveDir>
    {
        protected override async ETTask Run(Unit unit, C2M_MoveDir message)
        {
            unit.MoveDirAsync(message.Direction).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}

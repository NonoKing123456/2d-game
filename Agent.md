# 项目协作说明

## 项目概况

- 这是一个 Unity 6（`6000.6.3f1`）2D 游戏项目，使用 URP 2D、Input System 和 Tilemap。
- 场景位于 `Assets/Scenes/`；`MainMenu.unity` 是菜单入口，`Level1.unity` 是游戏关卡。
- 游戏逻辑集中在 `Assets/Scripts/`：`PlayerControl` 处理移动、跳跃和攻击，`PlayerAnimation` 负责动画参数，`PlayerLife` 负责生命值；`EnemyBrain`、`EnemyPatrol`、`EnemyChase`、`EnemyHealth` 负责敌人行为；`CameraFollow2D` 和 `ImgBackground` 负责镜头与背景。
- 输入资源位于 `Assets/Settings/InputSystem_Actions.inputactions`；渲染设置位于 `Assets/Settings/`。

## 修改约定

- 修改资源、场景、动画或脚本时，保留并一同提交对应的 `.meta` 文件；移动资源时优先在 Unity 编辑器中操作，避免 GUID 引用断裂。
- 不提交 `Library/`、`Temp/`、`Logs/`、`UserSettings/`、`GeneratedAssets/`、IDE 生成文件或构建产物。需要提交的项目内容主要是 `Assets/`、`Packages/`、`ProjectSettings/`。
- 改动序列化字段、对象名称、Tag、Layer、动画参数或输入动作前，检查场景和相关资源的引用。当前代码依赖 `Player` Tag、`Ground` Layer、`GroundCheck` 和 `AttackHitbox` 子对象，以及 `Move`、`Jump`、`Attack` 输入动作。
- 当前玩家动画使用 `Speed`、`Grounded`、`VerticalVelocity` 参数和 `Slash` 触发器。修改动画控制器或代码时保持两者一致。
- 保持现有 C# 命名和 Unity 生命周期方法的风格；新增行为尽量放在职责对应的脚本中，避免无关重构。

## 验证

- 用项目指定的 Unity 版本打开项目，确认脚本编译通过且 Console 没有新增错误。
- 涉及玩法时，从 `MainMenu` 进入 `Level1`，实际检查移动、跳跃、攻击、敌人巡逻与追逐，以及镜头表现。
- 如果当前环境无法启动 Unity，在交付说明中明确写出未验证的部分，不把纯文本检查当成运行验证。

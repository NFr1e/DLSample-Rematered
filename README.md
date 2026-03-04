# DLSample

这是一个基于 [Unity2022](https://unity.com/) 开发的，以游戏 [跳舞的线](https://www.cheetahgames.com/)为蓝本的项目，未来计划包含游戏Gameplay部分的完整游戏流程复刻实现。

## 一.环境配置

项目使用 [InputSystem](https://docs.unity3d.com/Manual/com.unity.inputsystem.html), [UI Toolkit](https://docs.unity3d.com/Manual/UIElements.html)等较新特性，不同版本可能出现API不兼容等情况。

- 最低版本 Unity 2019.4

- **推荐使用Unity2022.3 LTS**

### 付费插件
- [Odin Inspector](https://odininspector.com/) 编辑器拓展

## 二.项目内容

### DLSample工作流
DLSample内置编辑器拓展，计划实现完整便捷的关卡设计工作流。

#### BeatMapData
- 关卡的“时间数据源”，记录关卡的标准时间点。可通过TrackGrapher编辑。未来计划增加OSU谱面转译功能。

#### PathGrapher
- PathGrapher是DLSample提供的通过BeatpapData传入时间数据源，并根据时间数据模拟出关卡标准路径的可视化工具。设计师可在路径时间点上增删改路径事件(PathEvent)实现路径变换等。路径事件包含点性事件和段性事件。点性事件如SpeedChangeEvent, GravityChangeEvent等，只需提供起点。段性事件如JumpEvent，需要提供起点和终点。
- PathGrapher上创建的事件可直接通过PathGrapherEventsSyncer同步到Gameplay。

### 项目分支

- base分支仅包含DLSample中的基础功能实现，适合项目拓展
- main分支中包含DLSample中各部分的完整实现

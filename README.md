# Spine3551SkeltoJson

## Spine 3.5.51 的 二进制模型文件 *.skel 转换 Json 格式模型 GUI版

主要用于转换明日方舟的干员小人模型，使之可以被龙骨打开

本项目的诞生离不开[SpineBin2Json35](https://github.com/huix-oldcat/SpineBin2Json35) 

以及 [spine-csharp](https://github.com/EsotericSoftware/spine-runtimes/tree/3.5/spine-csharp) 3.5.51(此版本只能在[release](https://github.com/EsotericSoftware/spine-runtimes/releases/tag/3.5.51)处获取) 官方运行库的支持

本项目已经有GUI，可以较方便地使用

# 原项目README

## SpineBin2Json35

许可证 : GPL v3

将Spine二进制导出文件 v3.5 (最常见的是3.5.51) 转换为JSON格式.

这个程序并不是基于Spine运行库的.

Usage:

将两个文件(*.skel和*.atlas)拖拽到本程序上
两个*需要一样
程序会创建/覆盖*.json(导出文件) *.txt(龙骨不支持的特性)

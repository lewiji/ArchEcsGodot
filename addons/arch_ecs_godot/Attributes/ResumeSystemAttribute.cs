using System;

namespace ArchEcsGodot.Attributes;

public class ResumeSystemAttribute(string worldState, int priority = 0, Type? runAfter = null) : EcsSystemAttribute(worldState, priority, runAfter);
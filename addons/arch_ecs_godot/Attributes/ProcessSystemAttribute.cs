using System;

namespace ArchEcsGodot.Attributes;

public class ProcessSystemAttribute(string worldState, int priority = 0, Type? runAfter = null) : EcsSystemAttribute(worldState, priority, runAfter);
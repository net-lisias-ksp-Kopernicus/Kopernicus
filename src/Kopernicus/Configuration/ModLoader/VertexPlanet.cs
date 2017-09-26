﻿/**
 * Kopernicus Planetary System Modifier
 * ------------------------------------------------------------- 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301  USA
 * 
 * This library is intended to be used as a plugin for Kerbal Space Program
 * which is copyright 2011-2017 Squad. Your usage of Kerbal Space Program
 * itself is governed by the terms of its EULA, not the license above.
 * 
 * https://kerbalspaceprogram.com
 */

using LibNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kopernicus
{
    namespace Configuration
    {
        namespace ModLoader
        {
            [RequireConfigType(ConfigType.Node)]
            public class VertexPlanet : ModLoader<PQSMod_VertexPlanet>, IParserEventSubscriber
            {
                // Loader for the SimplexWrapper
                [RequireConfigType(ConfigType.Node)]
                public class SimplexWrapper : IParserEventSubscriber
                {
                    // Loaded wrapper
                    public PQSMod_VertexPlanet.SimplexWrapper wrapper;

                    // deformity
                    [ParserTarget("deformity")]
                    public NumericParser<Double> deformity
                    {
                        get { return wrapper.deformity; }
                        set { wrapper.deformity = value; }
                    }

                    // frequency
                    [ParserTarget("frequency")]
                    public NumericParser<Double> frequency
                    {
                        get { return wrapper.frequency; }
                        set { wrapper.frequency = value; }
                    }

                    // octaves
                    [ParserTarget("octaves")]
                    public NumericParser<Double> octaves
                    {
                        get { return wrapper.octaves; }
                        set { wrapper.octaves = value; }
                    }

                    // persistance
                    [ParserTarget("persistance")]
                    public NumericParser<Double> persistance
                    {
                        get { return wrapper.persistance; }
                        set { wrapper.persistance = value; }
                    }

                    // seed
                    [ParserTarget("seed")]
                    public NumericParser<Int32> seed = 0;

                    // Apply Event
                    void IParserEventSubscriber.Apply(ConfigNode node) { }

                    // Post Apply Event
                    void IParserEventSubscriber.PostApply(ConfigNode node)
                    {
                        wrapper.Setup(seed);
                    }

                    // Default constructor
                    public SimplexWrapper()
                    {
                        wrapper = new PQSMod_VertexPlanet.SimplexWrapper(0, 0, 0, 0);
                    }

                    // Runtime constructor
                    public SimplexWrapper(PQSMod_VertexPlanet.SimplexWrapper simplex)
                    {
                        wrapper = simplex;
                    }

                    // Convert
                    public static implicit operator PQSMod_VertexPlanet.SimplexWrapper(SimplexWrapper parser)
                    {
                        return parser?.wrapper;
                    }
                    public static implicit operator SimplexWrapper(PQSMod_VertexPlanet.SimplexWrapper value)
                    {
                        return value == null ? null : new SimplexWrapper(value);
                    }
                }

                // Loader for Noise
                [RequireConfigType(ConfigType.Node)]
                public class NoiseModWrapper : IParserEventSubscriber
                {
                    // The loaded noise
                    public PQSMod_VertexPlanet.NoiseModWrapper wrapper;

                    // Parser for RiggedMultifractal (Yes, you can use any ModuleBase, but I don't want to code them all...)
                    public class RiggedParser
                    {
                        // Noise
                        public RidgedMultifractal noise;

                        // frequency
                        [ParserTarget("frequency")]
                        public NumericParser<Double> frequency
                        {
                            get { return noise.Frequency; }
                            set { noise.Frequency = value; }
                        }

                        // lacunarity
                        [ParserTarget("lacunarity")]
                        public NumericParser<Double> lacunarity
                        {
                            get { return noise.Lacunarity; }
                            set { noise.Lacunarity = value; }
                        }

                        // octaveCount
                        [ParserTarget("octaveCount")]
                        public NumericParser<Int32> octaveCount
                        {
                            get { return noise.OctaveCount; }
                            set { noise.OctaveCount = Mathf.Clamp(value, 1, 30); }
                        }

                        // quality
                        [ParserTarget("quality")]
                        public EnumParser<KopernicusNoiseQuality> quality
                        {
                            get { return (KopernicusNoiseQuality) (Int32) noise.NoiseQuality; }
                            set { noise.NoiseQuality = (NoiseQuality) (Int32) value.value; }
                        }

                        // seed
                        [ParserTarget("seed")]
                        public NumericParser<Int32> seed
                        {
                            get { return noise.Seed; }
                            set { noise.Seed = value; }
                        }

                        // Default Constructor
                        public RiggedParser()
                        {
                            noise = new RidgedMultifractal();
                        }

                        // Runtime Constructor
                        public RiggedParser(RidgedMultifractal rigged)
                        {
                            noise = rigged;
                        }
                        
                    }

                    // deformity
                    [ParserTarget("deformity")]
                    public NumericParser<Double> deformity
                    {
                        get { return wrapper.deformity; }
                        set { wrapper.deformity = value; }
                    }

                    // frequency
                    [ParserTarget("frequency")]
                    public NumericParser<Double> frequency
                    {
                        get { return wrapper.frequency; }
                        set { wrapper.frequency = value; }
                    }

                    // octaves
                    [ParserTarget("octaves")]
                    public NumericParser<Int32> octaves
                    {
                        get { return wrapper.octaves; }
                        set { wrapper.octaves = Mathf.Clamp(value, 1, 30); }
                    }

                    // persistance
                    [ParserTarget("persistance")]
                    public NumericParser<Double> persistance
                    {
                        get { return wrapper.persistance; }
                        set { wrapper.persistance = value; }
                    }

                    // seed
                    [ParserTarget("seed")]
                    public NumericParser<Int32> seedLoader
                    {
                        get { return wrapper.seed; }
                        set { wrapper.seed = value; }
                    }

                    // noise
                    [ParserTarget("Noise", allowMerge = true)]
                    public RiggedParser riggedNoise { get; set; }

                    // Apply Event
                    void IParserEventSubscriber.Apply(ConfigNode node)
                    {
                        if (wrapper.noise is RidgedMultifractal)
                            riggedNoise = new RiggedParser((RidgedMultifractal)wrapper.noise);
                    }

                    // Post Apply Event
                    void IParserEventSubscriber.PostApply(ConfigNode node)
                    {
                        wrapper.Setup(riggedNoise.noise);
                    }

                    // Default constructor
                    public NoiseModWrapper()
                    {
                        wrapper = new PQSMod_VertexPlanet.NoiseModWrapper(0, 0, 0, 0);
                    }

                    // Runtime Constructor
                    public NoiseModWrapper(PQSMod_VertexPlanet.NoiseModWrapper noise)
                    {
                        wrapper = noise;
                    }

                    // Convert
                    public static implicit operator PQSMod_VertexPlanet.NoiseModWrapper(NoiseModWrapper parser)
                    {
                        return parser?.wrapper;
                    }
                    public static implicit operator NoiseModWrapper(PQSMod_VertexPlanet.NoiseModWrapper value)
                    {
                        return value == null ? null : new NoiseModWrapper(value);
                    }
                }

                // Land class loader 
                [RequireConfigType(ConfigType.Node)]
                public class LandClassLoader
                {
                    // Land class object
                    public PQSMod_VertexPlanet.LandClass landClass;

                    // Name of the class
                    [ParserTarget("name")]
                    public String name
                    {
                        get { return landClass.name; }
                        set { landClass.name = value; }
                    }

                    // Should we delete this
                    [ParserTarget("delete")]
                    public NumericParser<Boolean> delete = new NumericParser<Boolean>(false);

                    // baseColor
                    [ParserTarget("baseColor")]
                    public ColorParser baseColor
                    {
                        get { return landClass.baseColor; }
                        set { landClass.baseColor = value; }
                    }

                    // colorNoise
                    [ParserTarget("colorNoise")]
                    public ColorParser colorNoise
                    {
                        get { return landClass.colorNoise; }
                        set { landClass.colorNoise = value; }
                    }

                    // colorNoiseAmount
                    [ParserTarget("colorNoiseAmount")]
                    public NumericParser<Double> colorNoiseAmount
                    {
                        get { return landClass.colorNoiseAmount; }
                        set { landClass.colorNoiseAmount = value; }
                    }

                    // colorNoiseMap
                    [ParserTarget("SimplexNoiseMap", allowMerge = true)]
                    public SimplexWrapper colorNoiseMap
                    {
                        get { return landClass.colorNoiseMap; }
                        set { landClass.colorNoiseMap = value.wrapper; }
                    }

                    // fractalEnd
                    [ParserTarget("fractalEnd")]
                    public NumericParser<Double> fractalEnd
                    {
                        get { return landClass.fractalEnd; }
                        set { landClass.fractalEnd = value; }
                    }

                    // fractalStart
                    [ParserTarget("fractalStart")]
                    public NumericParser<Double> fractalStart
                    {
                        get { return landClass.fractalStart; }
                        set { landClass.fractalStart = value; }
                    }

                    // lerpToNext
                    [ParserTarget("lerpToNext")]
                    public NumericParser<Boolean> lerpToNext
                    {
                        get { return landClass.lerpToNext; }
                        set { landClass.lerpToNext = value; }
                    }

                    // fractalDelta
                    [ParserTarget("fractalDelta")]
                    public NumericParser<Double> fractalDelta
                    {
                        get { return landClass.fractalDelta; }
                        set { landClass.fractalDelta = value; }
                    }

                    // endHeight
                    [ParserTarget("endHeight")]
                    public NumericParser<Double> endHeight
                    {
                        get { return landClass.endHeight; }
                        set { landClass.endHeight = value; }
                    }

                    // startHeight
                    [ParserTarget("startHeight")]
                    public NumericParser<Double> startHeight
                    {
                        get { return landClass.startHeight; }
                        set { landClass.startHeight = value; }
                    }
                    
                    // Default constructor
                    public LandClassLoader()
                    {
                        landClass = new PQSMod_VertexPlanet.LandClass("class", 0.0, 0.0, Color.white, Color.white, 0.0);
                    }

                    // Runtime constructor
                    public LandClassLoader(PQSMod_VertexPlanet.LandClass land)
                    {
                        landClass = land;
                    }

                    // Convert
                    public static implicit operator PQSMod_VertexPlanet.LandClass(LandClassLoader parser)
                    {
                        return parser?.landClass;
                    }
                    public static implicit operator LandClassLoader(PQSMod_VertexPlanet.LandClass value)
                    {
                        return value == null ? null : new LandClassLoader(value);
                    }
                }

                // buildHeightColors
                [ParserTarget("buildHeightColors")]
                public NumericParser<Boolean> buildHeightColors 
                {
                    get { return mod.buildHeightColors; }
                    set { mod.buildHeightColors = value; }
                }

                // colorDeformity
                [ParserTarget("colorDeformity")]
                public NumericParser<Double> colorDeformity
                {
                    get { return mod.colorDeformity; }
                    set { mod.colorDeformity = value; }
                }

                // continental
                [ParserTarget("ContinentalSimplex", allowMerge = true)]
                public SimplexWrapper continental
                {
                    get { return mod.continental; }
                    set { mod.continental = value.wrapper; }
                }

                // continentalRuggedness
                [ParserTarget("RuggednessSimplex", allowMerge = true)]
                public SimplexWrapper continentalRuggedness
                {
                    get { return mod.continentalRuggedness; }
                    set { mod.continentalRuggedness = value.wrapper; }
                }

                // continentalSharpness
                [ParserTarget("SharpnessNoise", allowMerge = true)]
                public NoiseModWrapper continentalSharpness
                {
                    get { return mod.continentalSharpness; }
                    set { mod.continentalSharpness = value.wrapper; }
                }

                // continentalSharpnessMap
                [ParserTarget("SharpnessSimplexMap", allowMerge = true)]
                public SimplexWrapper continentalSharpnessMap
                {
                    get { return mod.continentalSharpnessMap; }
                    set { mod.continentalSharpnessMap = value.wrapper; }
                }

                // deformity
                [ParserTarget("deformity")]
                public NumericParser<Double> deformity
                {
                    get { return mod.deformity; }
                    set { mod.deformity = value; }
                }

                // landClasses
                public List<LandClassLoader> landClasses = new List<LandClassLoader>();

                // oceanDepth
                [ParserTarget("oceanDepth")]
                public NumericParser<Double> oceanDepth
                {
                    get { return mod.oceanDepth; }
                    set { mod.oceanDepth = value; }
                }

                // oceanLevel
                [ParserTarget("oceanLevel")]
                public NumericParser<Double> oceanLevel
                {
                    get { return mod.oceanLevel; }
                    set { mod.oceanLevel = value; }
                }

                // oceanSnap
                [ParserTarget("oceanSnap")]
                public NumericParser<Boolean> oceanSnap
                {
                    get { return mod.oceanSnap; }
                    set { mod.oceanSnap = value; }
                }

                // oceanStep
                [ParserTarget("oceanStep")]
                public NumericParser<Double> oceanStep
                {
                    get { return mod.oceanStep; }
                    set { mod.oceanStep = value; }
                }

                // seed
                [ParserTarget("seed")]
                public NumericParser<Int32> seed
                {
                    get { return mod.seed; }
                    set { mod.seed = value; }
                }

                // terrainRidgeBalance
                [ParserTarget("terrainRidgeBalance")]
                public NumericParser<Double> terrainRidgeBalance
                {
                    get { return mod.terrainRidgeBalance; }
                    set { mod.terrainRidgeBalance = value; }
                }

                // terrainRidgesMax
                [ParserTarget("terrainRidgesMax")]
                public NumericParser<Double> terrainRidgesMax
                {
                    get { return mod.terrainRidgesMax; }
                    set { mod.terrainRidgesMax = value; }
                }

                // terrainRidgesMin
                [ParserTarget("terrainRidgesMin")]
                public NumericParser<Double> terrainRidgesMin
                {
                    get { return mod.terrainRidgesMin; }
                    set { mod.terrainRidgesMin = value; }
                }

                // terrainShapeEnd
                [ParserTarget("terrainShapeEnd")]
                public NumericParser<Double> terrainShapeEnd
                {
                    get { return mod.terrainShapeEnd; }
                    set { mod.terrainShapeEnd = value; }
                }

                // terrainShapeStart
                [ParserTarget("terrainShapeStart")]
                public NumericParser<Double> terrainShapeStart
                {
                    get { return mod.terrainShapeStart; }
                    set { mod.terrainShapeStart = value; }
                }

                // terrainSmoothing
                [ParserTarget("terrainSmoothing")]
                public NumericParser<Double> terrainSmoothing
                {
                    get { return mod.terrainSmoothing; }
                    set { mod.terrainSmoothing = value; }
                }

                // terrainType
                [ParserTarget("TerrainTypeSimplex", allowMerge = true)]
                public SimplexWrapper terrainType
                {
                    get { return mod.terrainType; }
                    set { mod.terrainType = value.wrapper; }
                }
                
                // Apply Event
                void IParserEventSubscriber.Apply(ConfigNode node) { }

                // Post Apply Event
                void IParserEventSubscriber.PostApply(ConfigNode node)
                {
                    // Load the LandClasses manually, to support patching
                    if (!node.HasNode("LandClasses"))
                        return;

                    // Already patched classes
                    List<PQSMod_VertexPlanet.LandClass> patchedClasses = new List<PQSMod_VertexPlanet.LandClass>();
                    if (mod.landClasses != null)
                        mod.landClasses.ToList().ForEach(c => landClasses.Add(new LandClassLoader(c)));

                    // Go through the nodes
                    foreach (ConfigNode lcNode in node.GetNode("LandClasses").nodes)
                    {
                        // The Loader
                        LandClassLoader loader = null;

                        // Are there existing LandClasses?
                        if (landClasses.Count > 0)
                        {
                            // Attempt to find a LandClass we can edit that we have not edited before
                            loader = landClasses.Where(m => !patchedClasses.Contains(m.landClass) && ((lcNode.HasValue("name") ? m.landClass.name == lcNode.GetValue("name") : true) || (lcNode.HasValue("index") ? landClasses.IndexOf(m).ToString() == lcNode.GetValue("index") : false)))
                                                             .FirstOrDefault();

                            // Load the Loader (lol)
                            if (loader != null)
                            {
                                Parser.LoadObjectFromConfigurationNode(loader, lcNode, "Kopernicus");
                                landClasses.Remove(loader);
                                patchedClasses.Add(loader.landClass);
                            }
                        }

                        // If we can't patch a LandClass, create a new one
                        if (loader == null)
                        {
                            loader = Parser.CreateObjectFromConfigNode<LandClassLoader>(lcNode, "Kopernicus");
                        }

                        // Add the Loader to the List
                        if (!loader.delete.value)
                            landClasses.Add(loader);
                    }

                    // Apply the landclasses
                    mod.landClasses = landClasses.Select(l => l.landClass).ToArray();
                }
            }
        }
    }
}

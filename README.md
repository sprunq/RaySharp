# RaySharp

![Lucy Statue](https://raw.githubusercontent.com/sprunq/RaySharp/main/Pictures/lucy.png?token=AMNAZ7ZMV5VFYJHL7MXA6ITBHU27M "Lucy")

### What is this?

A C# raytracer, based on the book Ray Tracing in One Weekend by Peter Shirley, with additional live preview in OpenTK, Wavefront OBJ model loading and a denoiser.

### Why C#?

I mainly used this project to learn C# for an upcoming internship. You should choose C/C++ if you care more about performance.

### Adding the Denoiser

Download the compiled binaries or compile this [Project (DeclanRussell/NvidiaAIDenoiser)](https://github.com/DeclanRussell/NvidiaAIDenoiser) yourself and add it as a path in the Raytracer constructor.
This Denoiser only works well in some cases and makes the image pretty blurry.

|                                                            Without Denoiser                                                            |                                                            With Denoiser                                                            |
| :------------------------------------------------------------------------------------------------------------------------------------: | :---------------------------------------------------------------------------------------------------------------------------------: |
| ![](https://raw.githubusercontent.com/sprunq/RaySharp/main/Pictures/Denoise%20Example/without.png?token=AMNAZ74LFJGSBO37KQVK5NTBHU3HE) | ![](https://raw.githubusercontent.com/sprunq/RaySharp/main/Pictures/Denoise%20Example/with.png?token=AMNAZ7YRTQNT5WNROFWNLL3BHU3H2) |

### Sources

\- Basic Raytracer Structure [Ray Tracing in One Weekend](https://raytracing.github.io/books/RayTracingInOneWeekend.html)

\- Additional Info [Scratchapixel Raytracing](https://www.scratchapixel.com/lessons/3d-basic-rendering/introduction-to-ray-tracing)

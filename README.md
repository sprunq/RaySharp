# RaySharp

![Lucy Statue](https://raw.githubusercontent.com/sprunq/Raytracer/main/Pictures/lucy.png?token=AMNAZ72UMYDAMOVD6RXBFT3BHUWPS "Random Scene")

### What is this?

This project is a C# raytracer, which is based on the book Ray Tracing in One Weekend by Peter Shirley, with additional live preview in OpenTK,Wavefront OBJ model loading and a denoiser.

### Why C#?

I mainly used this project to learn C# for an upcoming internship. You should choose C/C++ if you care more about performance.

### Adding the Denoiser

Download the compiled binaries or compile this [Project (DeclanRussell/NvidiaAIDenoiser)](https://github.com/DeclanRussell/NvidiaAIDenoiser) yourself and add it as a path in the Raytracer constructor.
This Denoiser only works well in some cases and makes the image pretty blurry.

Without Denoiser | With Denoiser
:-------------------------:|:-------------------------:
![](https://raw.githubusercontent.com/sprunq/Raytracer/main/Pictures/Denoise%20Example/without.png?token=AMNAZ75MFCFGEZ345QNMY53BHUZJG) | ![](https://raw.githubusercontent.com/sprunq/Raytracer/main/Pictures/Denoise%20Example/with.png?token=AMNAZ75DS4K6KC7PW442MHDBHUZV2)

### Sources

\- Basic Raytracer Structure [Ray Tracing in One Weekend](https://raytracing.github.io/books/RayTracingInOneWeekend.html)

\- Additional Info [Scratchapixel Raytracing](https://www.scratchapixel.com/lessons/3d-basic-rendering/introduction-to-ray-tracing)

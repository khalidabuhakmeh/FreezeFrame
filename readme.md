# FreezeFrame - .NET Global Tool

![Freeze Frame logo](./logo.png)

This is a .NET global tool designed to grab the first frame of all GIFs in a given directory. Grabbing the first frame is useful for creating a splash image for websites that don't auto-play animated GIFs. This was also an attempt to learn some F# and build something I would use.

## Getting Started

```console
dotnet run -- --help
```

With a resulting output of

```console
FreezeFrame 1.0.0
Copyright (C) 2021 FreezeFrame

  -d, --dir          (Default: ./) Directory containing GIFs to process.

  -r, --recursive    (Default: false) Recursively scan directories from root directory

  --help             Display this help screen.

  --version          Display version information.

```

## Licenses For ImageSharp

- This tool uses [ImageSharp](https://sixlabors.com/products/imagesharp/), which requires a license for commercial support.

## License

The MIT License (MIT)
Copyright © 2021 Khalid Abuhakmeh

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

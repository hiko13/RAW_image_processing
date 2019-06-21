![header](C:\Users\HIKO\source\repos\raw_image_prc\RAW_image_processing\img\header.png)



# RAW_image_processing

Demosaic bayer raw image. And basic image processing with UI.



## About

This is simple raw image processing application intended to scientific, industrial imaging.

It's for specific use, so does not support general raw image formats (e.g. Nikon .NEF, Canon .CR2 file).



## File Formats

- .tif 16bit gray image
- .tif 16bit color image (now working on)



## Functions

- total gain, offset level adjust
- gain, offset adjust for each color channel.(RGB)
- gamma correction
- save image (now working on)
  - color image (only demosaic is processed)
  - each color channel image (R, G, B image detached from bayer raw image)
  - color image (gain, offset, gamma is applied)



## Roadmap

Functions to be implemented in the future.

- conversion of color image into monochrome
- display image switching 
- dark image subtraction
- horizontal/vertical averaging

- blur (mean, gaussian, median and custom filter convolution)
- edge enhance
- color matrix
- auto contrast adjustment
- auto WB adjustment
- color saturation control



## Algorithms

### color plane split

Split color plane from bayer image.
Each color plane image goes to be half-size of original raw image.



### demosaic

The simplest demosaic algorithm is implemented as follow.



### gamma correction


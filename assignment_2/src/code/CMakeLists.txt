cmake_minimum_required(VERSION 2.8.3)
project(code)

find_package(catkin REQUIRED COMPONENTS 
 roslib
 roscpp
 tf
 stdr_msgs
 geometry_msgs
 sensor_msgs
 nav_msgs
)


set(CMAKE_BUILD_TYPE Release)

include_directories(
 include
 ${catkin_INCLUDE_DIRS}
)

catkin_package(
 INCLUDE_DIRS
  .
 LIBRARIES
 CATKIN_DEPENDS
 roslib
 roscpp
 tf
 stdr_msgs
 geometry_msgs
 sensor_msgs
 nav_msgs
)


add_executable(hello hello.cpp robot_script.cpp robot_script.h)
target_link_libraries(hello ${catkin_LIBRARIES})


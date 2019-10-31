#include "robot_script.h"
using namespace std;

RobotScript::RobotScript(int argc,char **argv) {
		ros::NodeHandle nh;

		ros::Publisher pub = nh.advertise<geometry_msgs::Twist>("/robot0/cmd_vel", 1000);

		srand(time(0));

		ros::Rate rate(2);

		while(ros::ok()) {
			geometry_msgs::Twist msg;
			msg.linear.x = double(rand())/double(RAND_MAX);
			msg.angular.z = 2*double(rand())/double(RAND_MAX) - 1;

			pub.publish(msg);

			ROS_INFO_STREAM("Sending random velocity command: " << "linear=" << msg.linear.x << "angular=" << msg.angular.z);


			ros::Subscriber sub = nh.subscribe("/robot0/cmd_vel", 1000, &ubscriber);
			rate.sleep();
	}
}

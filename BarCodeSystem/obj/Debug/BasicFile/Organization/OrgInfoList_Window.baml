   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5      &     BarCodeSystem.OrgInfoList_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5      -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.    � Title$ 组织信息窗口��5   	   $	��800��6   $	��600��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6H    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5      $��1��6$   $��5q�61   
 �� View
 5      �� 5      .P�5      �� $��组织编码��6%    �� Width$ 80��63    �� DisplayMemberBinding 6>   �� $ OI_Code�6o   �� 5      $��组织名称��6%   $	 180��63    6?   �� $ OI_Name�6p   �� 5      $��备注��6%   $	 250��61    6=   �� $ 	OI_Remark�6o   5      5!      � 5"   
   $��2��6    � ColumnDefinitions 5#      �� 5$      $	��150 �6#   61   �� 5%      $	��150 �6#   61   �� 5&      $	��150 �6#   61   �� 5'      $	��150 �6#   61   .+�5)      �� -   5        $��
btn_Import��5)      $��显示U9组织信息��6'   $��30��6:   $	��100��6F   $��5q�6R   $��0��6v   6�   �� 5*      -   5        $��
btn_Delete��5*      $��删除信息��6'   $��30��66   $��80��6B   $��5q�6M   $��1��6q   6�   �� 5+      -   5        $��
btn_Modify��5+      $��修改信息��6'   $��30��66   $��80��6B   $��2��6g   6y   �� 5,      -   5         ��Name$ 	btn_Close��5,      $��关闭��6(   $��30��65   $��80��6A   $��3��6e   5.      5/      50      
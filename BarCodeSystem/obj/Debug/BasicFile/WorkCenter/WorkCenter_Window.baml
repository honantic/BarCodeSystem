   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   E   %    BarCodeSystem.WorkCenter_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   5my&clr-namespace:System;assembly=mscorlib     5   E   -   5          � Title$  工作中心��5   	   $	��800��6   $	��600��6#    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6    � 	Resources 5      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector��%   &	          5	   
   6,   5
      .��5   
   � .+�5   
   � -   5        $��TopGrid��5       � ColumnDefinitions 5      �� 5      6%    � RowDefinitions 5      �� 5      $��60 �6    60   �� 5      6"   �� 5      $��60 �6    60   .+�5      � -   5        $��Bottom��5      $��1��6"   .+�5      �� -   5        $��	listView1��5   ,   $��5,5,5,5q�6   #���   5      ��5   =   ��  �� View 5      �� 5      .P�5      �� $��工作中心编码��6-    �� DisplayMemberBinding 6=   �� 	 �� Path$	 department_code�6v   �� 5      $��工作中心名称��6-   
 �� Width$	
 200��6s    6=   �� $	 department_name�6�   �� 5      $��工作中心简称��6-   $
 80��6x    6=   �� $	 department_shortname�6�   �� 5      $��启用标识��6-   $
 80��6s    6@   �� $	 isvalidated�6�   �� 5      $��是否工作中心��6-   $
 60��6p    6=   �� $	 isworkcenter�6}   �� 5       $-��'是否按照领料单控制派工数量��6-   $	
 100��6{    6D   �� $	 isordercontroled�6�   �� 5!      $��最新修改时间��6-   $	
 100��6u    6?   �� $	 lastoperatetime�6�   �� 5"      $��最新修改人员��6-   $	
 100��6q    6=   �� $	 lastoperateby�6�   5#      5%      5&      �� 5'      $��0��6   $��2��6)    �� Orientation$ 
Horizontal=�66   .+�5(      �� -   5        $��
btn_Modify��5(   D   $��30��6   $��80��6&   $��	5 5 160 5q�61   $��修改信息��6V   6�   �� 5)      -   5        $��
btn_Import��5)   D   $��30��6   $��80��6&   $��	5 5 170 5q�61   $��导入工作中心��6V   6�   �� 5*      -   5        $��	btn_Close��5*      $��30��6*   $��80��66   $��关闭��6A   6h   5+      5-      5.      5/      
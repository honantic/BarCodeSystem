   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5      (    "BarCodeSystem.OrgInfoImport_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5      -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.    � Title$  U9组织信息导入窗口��5   	   $	��600��6   $	��600��6)    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6L    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5      $��1��6$   $��5q�61   #���   5      
 �� View
 5      �� 5      .P�5      �� $��选择此组织��6%    �� Width$ 70��64    �� CellTemplate# [ 6?   6k   �� 5      $��组织编码��6%    �� DisplayMemberBinding 63   �� $ OI_Code�6d   �� 5      $��组织名称��6%    63   �� $ OI_Name�6d   5      5!      � 5#   
   $��2��6    � ColumnDefinitions 5$      �� 5%      $	��150 �6#   61   �� 5&      $	��150 �6#   61   �� 5'      $	��150 �6#   61   �� 5(      $	��150 �6#   61   .+�5*      �� -   5        $��
btn_Import��5*      $��导入U9组织信息��6'   $��30��6:   $	��100��6F   $��5q�6R   $��2��6v   $��0��6�   6�   �� 5+      -   5        $��btn_AllSelect��5+      $��全选��6*   $��30��67   $��80��6C   $��2��6j   $��1��6w   6�   �� 5,      -   5        $��btn_ReSelect��5,      $��重选��6)   $��30��66   $��80��6B   $��2��6h   $��2��6u   6�   �� 5-      -   5        $��	btn_Close��5-      $��关闭��6&   $��30��63   $��80��6?   $��2��6c   $��3��6p   6�   5.      51      52      
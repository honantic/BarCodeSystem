   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   +    %BarCodeSystem.WorkCenterImport_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � Title$  工作中心导入��5   	   $	��600��6   $	��600��6%    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6[    � 	Resources 5      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation d� 	IsChecked �� Path��%�   &	        &	        5   
   6,   �� 5	   
   .R�5
      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      ��  6   �� $ 
IsSelected�5      5      5      .��5   
   � .+�5   
   �  � RowDefinitions 5      �� 5      $��60 �6    6.   �� 5      6"   �� 5      $��60 �6    6.   	 � ColumnDefinitions	 5      �� 5      $	��100 �6#   62   �� 5      6%   .+�5      ]� $��0��6   $��1��6(   .+�5      �� -   5        $��	btn_Fetch��5   E   $��30��6   $	��Top=�6%   $��5q�6:   $��U9部门清单��6V   
 � ToolTip$M
 G获取U9中的部门清单，条码系统中已经存在的不做显示��6g   6�   �� 5      -   5        $��	btn_Imprt��5   L   $��30��6   $	��Top=�6%   $��5 20 5 5q�6:   $��导入��6]   $*
 $将选中的信息导入条码系统��6j   6�   �� 5      -   5        $��
btn_Cancel��5      $��Bottom=�6+   $��30��6C   $��5 20 5 5q�6O   $��90��6a   $��关闭��6l   6�   5       ]� 5!      $��1��6   $��1��6(   .+�5"      �� -   5        $��	listview1��5"      #���   5#      $��5q�6Q   $	��Top=�6\   ��5"   ,   ��  �� View 5%      �� 5&      .P�5'      ��  �� Width$ 30��6.    �� CellTemplate# [ 6:   $��选择��6d   5(      �� 5)      $	 150��6-   $��工作中心编码��69    �� DisplayMemberBinding 6I   �� $ department_code�6�   �� 5*      $	 250��6-   $��工作中心名称��69    6I   �� $ department_name�6�   5+      5-      � 5.      -   5        $��progressbar1��5.      $��Bottom=�62   $��10��6J   $��5q�6V   
���/���$y�	Collapsed=�6z   6�   50      �� 51      $��1��6   $��2��6)   $	��Top=�66   $ 
Horizontal=�6K   $��50��6d   $��Center=�6p   .+�52      �� -   5        $��btn_ReverseSelect��52      $��反选��62   $��5q�6?   $��30��6J   $��90��6V   6�   �� 53      -   5        $��btn_SelectAll��53      $��全选��6.   $��5q�6;   $��30��6F   $��90��6R   6{   �� 54      -	   5        $��btn_Reselect��54      $��重选��6-   $��5q�6:   $��30��6E   $��90��6Q   6y   �� 55      -
   5        $��txtb_Search��55      $��30��6-   $	��100��69   $��	20 5 2 5 q�6E   $��14(�6Y   $
 搜索工作中心名字��6g   6|   �� 56      -   5        $��
btn_Search��56      $��搜索��6+   $
 搜索工作中心名字��68   $	��0 5q�6d   $��30��6q   6   57      58      59      5:      
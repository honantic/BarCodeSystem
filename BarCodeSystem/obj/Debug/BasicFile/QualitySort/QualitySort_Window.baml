   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   &     BarCodeSystem.QualitySort_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 质检分类��5   	   $	��800��6   $	��600��6#    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6F    � 	Resources 5
      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5   +   $��1��6   $��5q�6    
 �� View
 5      �� 5      .P�5      �� $��选择��6%    �� CellTemplate# [ 61    �� Width$ 30��6[   6h   �� 5      $��质检分类编号��6%   $	 200��6c    �� DisplayMemberBinding 65   �� $ QS_Code�6q   �� 5      $��质检分类名称��6%   $	 200��6c    65   �� $ QS_Name�6q   5      5!      � 5#   
   $��2��6    � ColumnDefinitions 5%      �� 5&      $��90 �6#   60   �� 5'      $��90 �6#   60   �� 5(      $��90 �6#   60   �� 5)      $��50 �6#   60   �� 5*      $��50 �6#   60   �� 5+      $��* �6#   6/   �� 5,      $��45 �6#   60   �� 5-      $��* �6#   6/   .+�5/      �� -   5        $��btn_Add��5/      $��30��6$   $��80��60   $��5q�6;   $��新增信息��6F   $��0��6k   6}   �� 50      -   5        $��
btn_Modify��50      $��30��6(   $��5q�64   $��80��6?   $��修改信息��6J   $��1��6r   6�   �� 51      -   5        $��
btn_Delete��51      $��30��6'   $��5q�63   $��80��6>   $��删除信息��6I   $��2��6X   6�   �� 52      -   5        $��btn_SelectAll��52      $��30��6+   $��5q�67   $��40��6B   $��全选��6M   $��3��6v   6�   �� 53      -   5        $��btn_ReSelect��53      $��30��6*   $��5q�66   $��40��6A   $��重选��6L   $��4��6t   6�   �� 54      -   5        $��txtb_SearchKey��54      $��30��6,   $��75��68   $��14(�6C   $��5q�6Q   $��5��6�   6�   �� 55      -	   5        $��
btn_Search��55      $��30��6(   $��40��64   $��0 5 5 5q�6?   $��搜索��6P   $��6��6v   6�   �� 56      -
   5        $��	btn_Close��56      $��30��6'   $��70��63   $
��5,15q�6>   $��关闭��6L   $��7��6q   6�   57      58      59      
   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   #    BarCodeSystem.ItemInfo_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.    � Title$ 料品信息中心��5   	   $	��800��6   $	��600��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��61    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5      $��1��6$   $��5q�61   #���   5      
 �� View
 5      �� 5      .P�5      �� $��料品编码��6%    �� Width$ 80��63    �� DisplayMemberBinding 6>   �� $ II_Code�6n   �� 5      $��料品规格��6%   $ 80��63    6>   �� $ II_Spec�6n   �� 5      $��料品型号��6%   $ 80��63    6>   �� $ 
II_Version�6q   �� 5      $��料品名称��6%   $ 80��63    6>   �� $ II_Name�6n   �� 5       $��计量单位ID��6%   $ 80��65    6@   �� $ 	II_UnitID�6r   �� 5!      $��计量单位编码��6%   $ 80��65    6@   �� $ II_UnitCode�6t   �� 5"      $��计量单位名称��6%   $ 80��65    6@   �� $ II_UnitName�6t   �� 5#      $��质检分类��6%   $ 80��63    6>   �� $ II_QualitySortName�6y   5$      5&      �� 5'   
   $ 
Horizontal=�6   $��2��6.   .+�5(      �� -   5        $��
btn_Import��5(      $��导入U9料品��6@   $��30��6Q   $��80��6]   $��3q�6h   6v   �� 5)      -   5        $��
btn_Modify��5)      $��修改信息��6@   $��30��6O   $��80��6[   $��3q�6f   6s   �� 5*      -   5        $��txtb_SearchKey��5*      $��80��6U   $��100,15,5,15q�6`   $��14(�6u   �� 5+      -   5        $��
btn_Search��5+      $��搜索��6@   $��30��6M   $��40��6Y   $��5q�6d   6q   �� 5,      -   5        $��	btn_Close��5,      $��关闭��6>   $��30��6K   $��80��6W   $��80 5 5 5q�6b   6w   5-      5.      5/      
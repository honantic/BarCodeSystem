   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   e7clr-namespace:BarCodeSystem.PublicClass.ValueConverters)BarCodeSystem.PublicClass.ValueConverters  5   	   $    BarCodeSystem.Warehouse_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   Ilocal7clr-namespace:BarCodeSystem.PublicClass.ValueConverters     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 仓库中心��5   	   $	��800��6   $	��600��6#    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6F    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� PathD   >BarCodeSystem.PublicClass.ValueConverters.TrueOrFalseConverter  boolConverter��%�   &	        &	      &	 �       5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5        5   
   6=   5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5      $��1��6$   $��5q�61   #���   6<   
 �� View
 5      �� 5      .P�5      �� $��选择��6%    �� CellTemplate# [ 61    �� Width$
 auto��6[   6j   �� 5      $��仓库编码��6%   $
 auto��6`    �� DisplayMemberBinding 63   �� $ W_Code�6o   �� 5      $��仓库名称��6%   $
 auto��6`    63   �� $ W_Name�6o   �� 5      $��仓库来源��6%   $
 auto��6k    63   �� $ W_SourceType_Show�6z   �� 5       $��是否启用��6%   $
 auto��6l    63   �� $ W_IsValidated_Show�6{   �� 5!      $��关联工作中心��6%   $
 auto��6n    65   �� $ WC_Department_Name�6}   �� 5"      $��是否默认��6%   $
 auto��6�    63   �� $ W_IsDefault� �� 	Converter# [ 6�   5#      5%      �� 5&   
   $��2��6   $ 
Horizontal=�6"   .+�5'      �� -   5        $��
btn_Import��5'      $��5q�6'   $��导入仓库信息��6K   $��30��6\   $��80��6h   6u   �� 5(      -   5        $��btn_SelectAll��5(      $��30��6F   $��40��6R   $��5q�6]   $��全选��6h   6w   �� 5)      -   5        $��btn_ReSelect��5)      $��30��6D   $��40��6P   $��5q�6[   $��重选��6g   6v   �� 5*      -   5        $��btn_ValidateSelectedItem��5*      $��5 q�66   $��30��6i   $��80��6u   $��启用选中仓库��6�   6�   �� 5+      -   5        $��
btn_Modify��5+      $��5 q�6(   $��修改仓库信息��6M   $��30��6^   $��80��6j   6w   �� 5,      -   5        $��txtb_SearchKey��5,      $��5q�6,   $��30��6`   $��80��6l   $��14(�6w   6�   �� 5-      -	   5        $��
btn_Search��5-      $��30��6@   $��40��6L   $��搜索��6W   6f   �� 5.      -
   5        $��	btn_Close��5.      $��30��6>   $��80��6J   $��关闭��6U   $��5q�6b   6o   5/      50      51      
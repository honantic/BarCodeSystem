   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   *    $BarCodeSystem.WarehouseImport_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 仓库导入窗体��5   	   $	��600��6   $	��600��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6H    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5      $��1��6$   #���   61   
 �� View
 5      �� 5      .P�5      �� $��选择��6%    �� CellTemplate# [ 61   6]   �� 5      $��仓库编码��6%    �� Width$	 100��6`    �� DisplayMemberBinding 63   �� $ W_Code�6n   �� 5      $��仓库名称��6%   $	 150��6`    63   �� $ W_Name�6n   �� 5      $��仓库ID��6%   $	 150��6`    65   �� $
 W_ID�6n   �� 5      $��仓库信息来源��6%   $	 150��6j    67   �� $ W_SourceType�6x   5      5!      �� 5"   
   $��2��6   $ 
Horizontal=�6"   .+�5#      �� -   5        $��btn_FetchU9List��5#      $��30��6J   $��80��6V   $��5q�6a   $��U9仓库清单��6l   6   �� 5$      -   5        $��btn_SelectAll��5$      $��30��6F   $��40��6R   $��5q�6]   $��全选��6h   6w   �� 5%      -   5        $��btn_ReverseSelect��5%      $��30��6N   $��40��6Z   $��5q�6e   $��反选��6p   6   �� 5&      -   5        $��btn_ReSelectAll��5&      $��30��6J   $��40��6V   $��5q�6a   $��重选��6l   6{   �� 5'      -   5        $��txtb_Search��5'      $��14(�6)   $��30��67   $��30 5 5 5q�6C   $��80��6U   6�   �� 5(      -   5        $��
btn_Search��5(      $��30��6@   $��40��6L   $��5q�6W   $��搜索��6b   6q   �� 5)      -	   5        $��
btn_Improt��5)      $��30��6@   $��导入��6L   $��5q�6Y   $��80��6d   6r   �� 5*      -
   5        $��	btn_Close��5*      $��30��6>   $��80��6J   $��关闭��6U   $��5q�6b   6o   5+      5,      5-      
   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   e7clr-namespace:BarCodeSystem.PublicClass.ValueConverters)BarCodeSystem.PublicClass.ValueConverters  5   	   J    DBarCodeSystem.TechRoute.TechRoute.TechRouteWorkHourImportItem_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   Kconvert7clr-namespace:BarCodeSystem.PublicClass.ValueConverters     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5       � 
ResizeMode$ NoResize=�5   	    � Title$6 0工艺路线工时批量修改料品选择界面��5   	   $	��800��6"   $	��600��6/    � 	Resources 5
      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5       $��1��6   	 �� View	 5      �� 5      .P�5      �� $��选择��6%   
 �� Width$
 70��61    �� CellTemplate# [ 6<   6h   �� 5      $��料号��6%   $	
 100��61    �� DisplayMemberBinding 6=   �� *	II_Code+6h   �� 5      $��名称��6%   $	
 130��61    6=   �� *	II_Name+6h   �� 5      $��规格��6%   $	
 150��61    6=   �� *	II_Spec+6h   �� 5       $��型号��6%   $	
 150��61    6=   �� *
II_Version+6k   5!      5$      � 5'   
   $��2��6    � ColumnDefinitions 5(      �� 5)      6%   �� 5*      6%   �� 5+      6%   �� 5,      6%   .+�5/      �� -   5        $��btn_Confirm��5/   K   $��确认选择��6   $��80��6$   $��30��6/   $��0��6;   6z   �� 50      -   5        $��btn_AllSelect��50   I   $��全选��6   $��80��6"   $��30��6-   $��1��69   6|   �� 51      -   5        $��btn_ReSelect��51   I   $��重选��6   $��80��6"   $��30��6-   $��2��69   6z   �� 52      -   5        $��	btn_Close��52   I   $��关闭��6   $��80��6"   $��30��6-   $��3��69   6t   53      54      55      
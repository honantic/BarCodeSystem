   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      H DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulle7clr-namespace:BarCodeSystem.PublicClass.ValueConverters)BarCodeSystem.PublicClass.ValueConverters 5      E   ?BarCodeSystem.TechRoute.TechRouteWorkHourManually.WorkHour_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      0sys&clr-namespace:System;assembly=mscorlib  5      Clocal7clr-namespace:BarCodeSystem.PublicClass.ValueConverters  5      -   5          N� Title$  工时维护页面��5       � 	Resources 5      D  >BarCodeSystem.PublicClass.ValueConverters.TrueOrFalseConverter   boolConvert��%   &	          5   
   6;   5      .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      6   .+�5   
   �� $��3��6   .��5      �� $��15(�6   
�������工时维护页面6A   ��5      �� 5      ��5      �  � Offset$ 1Q�6(    � Color$ 
LightGreen��63   6H   � 5      $ Green��6'   67   5      5      � 5"   
   $��1��6    � ColumnDefinitions 5#      �� 5$      $	��200 �6#   61   �� 5%      $	��300 �6#   61   �� 5&      $	��200 �6#   61   �� 5'      6%   .+�5)      �� ��5*      �� 5+        textb_NormalStyle#��[ 6!   选择车间6R   .��5-      � -   5        $��grid_WorkCenterSelect��5-      5.      5/      �� 50      $��1��6   ��51      �� 52      #��[ 6!   选择料品6R   .��54      �  55      �� 56      $��50 �6(   66   �� 57      6*   .+�59      ��  �� Orientation$ 
Horizontal=�6#   $��Center=�6<   $��30��6W   .+�5:      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5        $��txtb_ItemInfo��5:   S      	Watermark$ 输入搜索信息��62   $	��0 5q�6E   $	��160��6i   
�������.��6�   5;      �� 5<      -   5        $��btn_ItemSearch��5<   !    � Cursor$
 Hand��67     btn_SearchStyle#��[ 6b   
�������
���������6�   �� 	 �� Path$	 ActualHeight�
 �� ElementName$
 txtb_ItemInfo��.��5=      �� $��15��6$   $��../../Images/Zoom.ico��60   6Q   5>      5?      &   System.Windows.Controls.DataGrid  5@      -   5        $��dg_ItemInfo��5@         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 62   $��1��6E      AutoGenerateColumns .     
IsReadOnly .    CanUserAddRows .     CanUserDeleteRows .     Columns 5A      0  *System.Windows.Controls.DataGridTextColumn  5B      ,  &System.Windows.Controls.DataGridColumn   Header$ 料号��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� $	 II_Code�6`     5C      $ 规格��61    6=   �� $	 II_Spec�6`     5D      $ 名称��61    6=   �� $	 II_Name�6`   5F      5G      5H      �� 5J      $��2��6   ��5K      �� 5L      #��[ 6!   选择工艺路线6T   .��5N      �  5O      �� 5P      $��50 �6(   66   �� 5Q      6*   .+�5S      �� $ 
Horizontal=�6#   $��Center=�6<   $��30��6W   .+�5T        -   5        $��txtb_TechRouteInfo��5T   2   $ 输入搜索信息��6L   $	��0 5q�6_   $	��160��6n   
�������.��6�   5U      �� 5V      -   5        $��btn_TechRouteSearch��5V   !   $
 Hand��6<   #��[ 6l   
�������
���������6�   �� $	 ActualHeight�$
 txtb_ItemInfo��.��5W      �� $��15��6$   $��../../Images/Zoom.ico��60   6Q   5X      5Y        5Z      -   5        $��dg_TechVersion��5Z      $
 Auto 5[      $��1��62    .   .  .   .   5]        5^      $ 版本名称��61    6?   �� $	 TRV_VersionName�6j     5_      $ 版本编码��61    6?   �� $	 TRV_VersionCode�6j   5a      5b      5c      �� 5e      $��3��6   ��5f      �� 5g      #��[ 6!   修改工时6R   .��5i      �  5j      �� 5k      $��50 �6(   66   �� 5l      6*   �� 5m      $	��100 �6(   67   .+�5p      �  5q      �� 5r      61   �� 5s      61   �� 5t      61   �� 5u      61   .+�5w      �� $��设置生效时间：��6$   $��Center=�63   $��Right=�6N   6l   (  "System.Windows.Controls.DatePicker  5x      -	   5        $��dp_StartDate��5x   %   {�.  $��1��6M   $��Center=�6]   $
 Hand��6y   
 �� Now   SelectedDate# Z 6�   6�   �� 5y      $��设置失效时间：��6$   $��2��63   $��Center=�6C   $��Right=�6^   6|     5z      -
   5        $��
dp_EndDate��5z   %   {�.  # Z 6J   $��3��6v   $��Center=�6�   $
 Hand��6�   6�   5{        5|      -   5        $��dg_TechRouteInfo��5|       . $��5q�6I   $��1��6T      RowHeaderWidth$ 30Q�6a      	RowHeight$ 40Q�6u    .   .   .     CanUserSortColumns .   5~        5      $ 行号��62      Width$
 Auto 6?      
IsReadOnly .  6L   �� $	 TR_ProcessSequence�6�     5�      $ 工序名称��61   $
 Auto 6?    .  6L   �� $	 TR_ProcessName�6�   4	  .System.Windows.Controls.DataGridTemplateColumn	  5�      $ 工时��65   $ 60 6A    	  CellTemplate 5�   "   �� 5�   &   .R�5�   *   �� ��64   �� $	 WorkHour�6U   5�   '    	  CellEditingTemplate 5�   "   �� 5�   &   .R�5�   *   %
  Xceed.Wpf.Toolkit.DecimalUpDown
  �  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   	Increment$
 0.01X�6?   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 0X�6P      Value 6\   �� $	 WH_WorkHour� �� Mode$ TwoWay=�6�   5�   '   5�        5�      $ 工作中心��61   $
 Auto 6?    .  6L   �� $	 WC_Department_Name�6�     5�      $ 版本编码��61   $
 Auto 6?    .  6L   �� $	 TRV_VersionCode�6�     5�      $ 版本名称��61   $
 Auto 6?    .  6L   �� $	 TRV_VersionName�6�     5�      $ 料品编码��61   $
 Auto 6?    .  6L   �� $	 TR_ItemCode�6�     5�      $ 	检验员��61   $
 Auto 6@    .  6M   �� $	 TR_DefaultCheckPersonName�6�   5�      � 5�      $��2��6    5�      �� 5�      61   �� 5�      61   �� 5�      61   �� 5�      61   �� 5�      61   .+�5�      �� -   5        $��btn_ViewHistory��5�   2   $��对比历史数据��6!   $��10q�6I     �� 	ClickMode$  Release=�6t   $	��Top=�6�   $��Center=�6�   $��5q�6�   6�   �� 5�      -   5        $��
btn_AddNew��5�   "   $��1��6M   $
��30 5q�6]   $��新增��6l   $	��Top=�6y   $��10q�6�   $��Center=�6�   6�   �� 5�      -   5        $��
btn_Cancel��5�   !   $��10q�6L   $  Release=�6Y   $��放弃��6m   $��3��6z   $	��Top=�6�   $��Center=�6�   $
��30 5q�6�   6�   �� 5�      -   5        $��btn_Save��5�   !   $��10q�6H   $  Release=�6U   $��保存��6i   $��4��6v   $	��Top=�6�   $��Center=�6�   $
��30 5q�6�   6�   �� 5�      -   5        $��btn_Rewrite��5�   .   $��重写��6!   $��10q�6A   $
��30 5q�6h   $	��Top=�6w   $��Center=�6�   $��2��6�   6�   5�      5�      5�      5�      5�      5�      
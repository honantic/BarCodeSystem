   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       BarCodeSystem*   $BarCodeSystem.BadProductSummary_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f47toolkit)http://schemas.xceed.com/wpf/xaml/toolkit  5      0sys&clr-namespace:System;assembly=mscorlib  5      -   5          N� Title$  不良品汇总表��5
       N� ShowsNavigationUI .  .��5      � -   5        $��father��5       � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��2* �6   6*   �� 5      $��15 �6   6*   �� 5      $��4* �6   6*   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��10 �6   6,   �� 5      $
��3.5* �6   6.   .+�5   
   �� $��1��6   $��Center=�6'   $��Stretch=�6D   $��5��6`   $��1��6k   $��3��6y   $��0 10 0 0q�6�   ��5      �� 5      .��5      �  � Color$ 	LightBlue��6#    � Offset$	 0.8Q�65   6D   � 5       $ Salmon��6#   $	 0.4Q�62   6A   � 5!      $ 
LightGreen��6#   68   5"      5$      �� 5'   
   $��3��6   .��5(      �� $��15(�6   
�������#!不良品明汇总表查询页面6F   ��5)      �� 5*      ��5,      � $ 1Q�6(   $ 
LightGreen��63   6H   � 5-      $ Green��6'   67   5/      51      �� 52   
   $��2��6   $��5��6$   
�������$��Center=�6G   $��Stretch=�6b   $
��5 10q�6�   6�   � 53   
   $��1��6   .+�54      �� -   5        $��	gb_header��54      ��55      �� 56      $$��不良品汇总表查询条件��6       textb_NormalStyle#��[  62   6_   .��59      � -   5        $��BadProductHeadGrid��59       5:      �� 5;      6*   �� 5<      6*   �� 5=      6*   �� 5>      6*    5A      �� 5B      6-   �� 5C      $��2* �6+   68   .+�5H      �� $��选择车间:��6    $��0 0 5 0q�6-   $��Center=�6>   $��Right=�6Y   $��0��6u   $��0��6�   6�   �� 5I      $��0��6!   $��1��6.   $
��Left=�6>   $��5q�6Y    �� Orientation$ 
Horizontal=�6�   $��Center=�6�   $��30��6�   ��6d   ��  �� Path$ ActualWidth� �� ElementName$ comobox_year��.+�5J      �� -   5        $��txtb_DeptInfo��5J   `   
�������$��Center=�69   $	��140��6T   	 �� 
IsReadOnly	 . $��点击放大镜选择��6�   
�������6�   �� 5L      -   5        $��btn_DeptSearch��5M   !   
 � Cursor$

 Hand��5L   !     btn_SearchStyle#��[ 6/   $��30��6Y   
�������
���������6x   �� $ ActualHeight�$ txtb_DeptInfo��.��5N      �� $��15��6$   $��/Images/Zoom.ico��60   6L   5O      5P      �� 5R      $��起始日期：��6    $��1��6-   $��0��6:   $��0 0 5 0q�6J   $��Center=�6[   $��Right=�6v   6�   (  "System.Windows.Controls.DatePicker  5S      -   5        $��datepicker_StartDate��5S   "   $	��180��6?   $

 Hand��6K   $��1��6Y   $��1��6i   $
��Left=�6v   $��Center=�6�   6�   �� 5U      $��结束日期：��6    $��2��6-   $��0��6:   $��0 0 5 0q�6J   $��Center=�6[   $��Right=�6v   6�     5V      -   5        $��datepicker_EndDate��5V   "   
 �� Now   SelectedDate# Z 6<   $	��180��6h   $

 Hand��6t   $��1��6�   $��2��6�   $
��Left=�6�   $��Center=�6�   6�   �� 5X      $��料品型号：��6    $��3��6-   $��0��6:   $��0 0 5 0q�6J   $��Center=�6[   $��Right=�6v   6�   (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5Y      -	   5        $��txtb_SearchKey��5Y   /   $��Center=�6E   $��14(�6`   $��Center=�6n   $��3��6�   $��1��6�   $
��Left=�6�   $	��180��6�   6�   5Z      5[      5\      � 5^   
   $��3��6   .+�5_      �� $��5q�6   ��5`      �� 5a      #��[  6!   车间信息6S   .��5d      � -
   5        $��DeptGrid��5d       5e      �� 5f      -   5         �Name$ row_top��5f   (   69   �� 5g      $��30 �6(   66   .+�5j      &   System.Windows.Controls.DataGrid  -   5        $��datagrid_Dept��5k   e      ClipboardCopyMode$
 None=�5j         CanUserDeleteRows .     CanUserReorderColumns .     CanUserSortColumns .     CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5k   R      RowHeaderWidth$ 30Q�5l         Columns 5m      0  *System.Windows.Controls.DataGridTextColumn  5n      ,  &System.Windows.Controls.DataGridColumn   Header$ 车间编码��61      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6Q   �� $ department_code�6|     5o      $ 车间名称��61    .  6Q   �� $ department_name�6|     5p      $ 简称��61    .  6P   �� $ department_shortname�6�   5r      � 5t      $��1��6    5u      �� 5v      61   �� 5w      61   �� 5x      61   .+�5{      �� -   5        $��	btn_Chose��5{   L   $��选择��6!   $	��120��6.   $��80��6=   $��0��6]   $��Center=�6n   $
��Left=�6�   $��80��6�   6�   �� 5|      -   5        $��	btn_Query��5|   L   $��查询��6!   $	��120��6.   $��80��6=   $��2��6]   $��Center=�6n   $��Right=�6�   $��80��6�   6�   5}      5      5�      5�      �� 5�   
   $��1��6   $��2��6    $��3��60   ��5�      �� 5�      $��不良品汇总表��6   #��[  6*   6W   .��5�       � -   5        $��frame_Search��5�        � JournalOwnership$ OwnsJournal=�6(     � NavigationUIVisibility$ Hidden=�6G   $��10q�6g   $��2��6s   $��1��6�   $��3��6�   6�   5�      5�      5�      
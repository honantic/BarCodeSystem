   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   ,    &BarCodeSystem.TechRouteItemList_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 料品清单��5	   	   $	��600��6   $	��400��6#    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6/    � 	Resources 5
      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector��%   &	          5   
   6,   5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   .+�5   
   �� -   5        $��	listview1��5      $��5q�6$   $��1��6/   $��3��6<   #���   5      ��6M   ��  �� View 5      �� 5      .P�5      �� $��料品编码��6%    �� Width$	 180��63   	 �� DisplayMemberBinding	 6?   �� 
 �� Path$
 II_Code�6o   �� 5      $��料品名称��6%   $	 180��63   	 6?   �� $
 II_Name�6o   5      5       (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5!   
   -   5          Name$ txtb_SearchKey��5!   "      	Watermark$ 回车搜索料号��6:   $��14(�6p   $��2��6~   $��30��6�   $
��10 0q�6�   6�   �� 5"   
   -   5        $��	btn_Chose��5"      $��2��6:   $��1��6G   $��30��6W   $��80��6c   $��选择��6n   6}   �� 5#   
   -   5        $��	btn_Close��5#      $��2��6:   $��2��6G   $��30��6W   $��80��6c   $��关闭��6n   6}   5$      5%      
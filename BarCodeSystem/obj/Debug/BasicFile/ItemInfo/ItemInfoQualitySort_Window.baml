   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   A    ;BarCodeSystem.BasicFile.ItemInfo.ItemInfoQualitySort_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 质检分类清单��5   	   $	��600��6   $	��400��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6H    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector��%   &	          5
   
   6,   5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   �� -   5        $��	listview1��5      $��1��6%   $��2��62   $��5q�6F   #���   6Q    �� View 5      �� 5      .P�5      �� $��质检分类编码��6%    �� Width$	 180��65   	 �� DisplayMemberBinding	 6A   �� 
 �� Path$
 QS_Code�6q   �� 5      $��质检分类名称��6%   $	 180��65   	 6A   �� $
 QS_Name�6q   5      5      �� 5!   
   -   5        $��	btn_Chose��5!      $��2��6"   $��30��6/   $��80��6;   $��选择��6F   6m   �� 5"   
   -   5        $��	btn_Close��5"      $��2��6"   $��1��6/   $��30��6?   $��80��6K   $��关闭��6V   6}   5#      5$      